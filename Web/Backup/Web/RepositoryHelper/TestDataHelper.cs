using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Hosting;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using AzureHelper.Authentication;
using AzureHelper.Security;
using BricsWeb.LocalModels;
using BricsWeb.Models;
using BricsWeb.Repository;

namespace BricsWeb.RepositoryHelper
{
    public class TestDataHelper
    {
        List<string> categoryList = new List<string>();

        public void UploadTestData()
        {
            Debug.WriteLine("Starting Sample Data Generation");

            Debug.WriteLine("Create Tables");
            CreateTables();

            Debug.WriteLine("Add test user");
            CreateTestUserAccount();

            Debug.WriteLine("Insert Categories");
            InsertCategoriesFromJsonBackup();

            Debug.WriteLine("Getting assignable categories (those without children)");
            LoadAssignableCategories();

            Debug.WriteLine("Adding sample data");
            GenerateSampleData();

            Debug.WriteLine("Completed Sample Data Generation");
        }

        private void InsertCategoriesFromJsonBackup()
        {
            var jsonBackup = File.ReadAllText(HostingEnvironment.ApplicationPhysicalPath + @"/Data/backup.json");
            
            var serializer = new DataContractJsonSerializer(typeof(BackupModel));
            var stream = new MemoryStream(Encoding.UTF8.GetBytes(jsonBackup));
           
            BackupModel backupData = serializer.ReadObject(stream) as BackupModel;

            backupData.CategoryList.ToList().ForEach(c =>
                {
                    CategoryModel category = new CategoryModel(c.RowKey, c.Name, c.Description, c.Depth, c.ParentID);
                    new CategoryRepository().Save(category);
                });

        }

        private void LoadAssignableCategories()
        {
            var categories = (from x in new CategoryRepository().GetAll()
                              select x.RowKey).ToList();

            foreach (string categoryName in categories)
            {
                var result = new CategoryRepository().CheckForChildren(categoryName);

                if (!result)
                {
                    categoryList.Add(categoryName);
                }
            }
        }

        private void CreateTables()
        {
            var productsExist = new ProductRepository().GetTableClient().DoesTableExist("Product");
            //var usersExist = new AzureUserRepository().GetTableClient().DoesTableExist("AzureUserTable");

            if (!productsExist)
            {
                new BuyerRequestRepository().ResetOrInitialise();
                new CompanyRepository().ResetOrInitialise();
                new CompanySubscriptionRepository().ResetOrInitialise(); 
                new ProductRepository().ResetOrInitialise();
                new TransactionRepository().ResetOrInitialise();
                new AzureRoleRepository().ResetOrInitialise();
                new AzureUserRepository().ResetOrInitialise();
                new CategoryRepository().ResetOrInitialise();
            }
            else
            {
                throw new Exception(@"Production Product Data Table Detected - Cannot Continue");
            }
        }

        private void CreateTestUserAccount()
        {
            AzureUser testAzureUser = new AzureUser
            {
                SessionTimeout = 60,
                IsLockedOut = false,
                IsOnline = false,
                LastActivityDate = DateTime.Now,
                LastLockoutDate = new DateTime(1900, 1, 1),
                LastPasswordChangedDate = new DateTime(1900, 1, 1),
                LastLoginDate = new DateTime(1900, 1, 1),
                RealName = "Jack Daniels",
                Email = "testaccount@crystaltouch.co.za",
                CreationDate = DateTime.Now,
                IsApproved = true,
                Password = "1234567",
                Username = "Jack",
                PartitionKey = "j",
                RowKey = Guid.NewGuid().ToString()
            };

            //save test user
            new AzureUserRepository().Save(testAzureUser);

            string roleID = Guid.NewGuid().ToString();
            AzureRole azureUserRole = new AzureRole
            {
                RowKey = roleID,
                RoleName = "Webmaster",
                PartitionKey = roleID.Substring(0, 1),
                UserRowKey = testAzureUser.RowKey,
                Timestamp = DateTime.Now
            };

            //save a role for the test user
            new AzureRoleRepository().Save(azureUserRole);
        }

        public string GetRandomCategory(List<string> categoryList)
        {
            Random rnd = new Random();
            int index = rnd.Next(0, categoryList.Count());

            return categoryList[index];
        }

        /// <summary>
        /// Create square black image with name text
        /// </summary>
        /// <param name="Name"></param>
        /// <returns></returns>
        private byte[] CreateImage(string Name)
        {
            Canvas canvas = new Canvas();
            canvas.Width = 120;
            canvas.Height = 120;
            canvas.Background = Brushes.Black;
            TextBlock tb = new TextBlock();
            tb.Text = Name;
            tb.Foreground = Brushes.White;
            tb.HorizontalAlignment = HorizontalAlignment.Center;
            tb.VerticalAlignment = VerticalAlignment.Center;
            tb.FontSize = 20;
            canvas.Children.Add(tb);

            Rect rect = new Rect(new Size(120, 120));
            RenderTargetBitmap rtb = new RenderTargetBitmap((int)rect.Right,
              (int)rect.Bottom, 96, 96, System.Windows.Media.PixelFormats.Pbgra32);
            canvas.Arrange(new Rect(0, 0, 120, 120));
            rtb.Render(canvas);
            //endcode as PNG
            BitmapEncoder pngEncoder = new PngBitmapEncoder();
            pngEncoder.Frames.Add(BitmapFrame.Create(rtb));

            //save to memory stream
            System.IO.MemoryStream ms = new System.IO.MemoryStream();

            pngEncoder.Save(ms);
            ms.Close();

            //System.IO.File.WriteAllBytes("logo.png", ms.ToArray());

            return ms.ToArray();

        }

        private void GenerateSampleData()
        {
            for (int i = 0; i < 200; i++)
            {
                //Create Users
                string rndUser = RandomString(5);
                AzureUser au = new AzureUser
                {
                    SessionTimeout = 60,
                    IsLockedOut = false,
                    IsOnline = false,
                    //Timestamp = DateTime.Now,
                    LastActivityDate = DateTime.Now,
                    LastLockoutDate = new DateTime(1900, 1, 1),
                    LastPasswordChangedDate = new DateTime(1900, 1, 1),
                    LastLoginDate = new DateTime(1900, 1, 1),
                    RealName = rndUser,
                    Email = rndUser + "@testuserdomain.com",
                    CreationDate = DateTime.Now,
                    IsApproved = true,
                    Password = "1234567",
                    Username = rndUser + "@testuserdomain.com",
                    PartitionKey = rndUser.Substring(0, 1),
                    RowKey = Guid.NewGuid().ToString()
                };
                new AzureUserRepository().Save(au);

                string UID = Guid.NewGuid().ToString();
                AzureRole arm = new AzureRole
                {
                    RowKey = UID,
                    RoleName = "RegisteredUser",
                    PartitionKey = UID.Substring(0, 1),
                    UserRowKey = au.RowKey,
                    Timestamp = DateTime.Now
                };

                new AzureRoleRepository().Save(arm);

                //Create Companies
                string rndCompanyName = RandomString(5);
                var company = new CompanyModel(au.RowKey, rndCompanyName, "sales@" + rndCompanyName + ".com", "accounts@" + rndCompanyName + ".com", i.ToString());
                company.Country = "South Africa";
                new CompanyRepository().Save(company);

                //Create 10 Products
                for (int x = 0; x < 10; x++)
                {
                    ProductModel product;
                    byte[] photo = null;
                    Thread printThread = new Thread(() =>
                    {
                        photo = CreateImage(RandomString(2));
                    });

                    printThread.SetApartmentState(ApartmentState.STA);
                    printThread.Start();
                    printThread.Join();
                    
                    string CatID = GetRandomCategory(categoryList);
                    string photoUrl = new PhotoRepository().SavePhoto(photo, Guid.NewGuid().ToString() + "_Test.png");
                    product = new ProductModel(RandomString(6), NextLoremIpsum(10),photoUrl,company.RowKey);
                    product.CategoryID = CatID;
                    product.Description = product.Description + " " + CatID;
                    product.FobPrice = RandomInt().ToString();
                    product.MinimumOrderQuantity = RandomInt();
                    product.ModelNumber = RandomInt().ToString();
                    product.PackagingAndDelivery = NextLoremIpsum(3);
                    product.PaymentTerms = NextLoremIpsum(4);
                    product.PlaceOfOrigin = NextLoremIpsum(2);
                    product.Port = NextLoremIpsum(1);
                    product.Quality = NextLoremIpsum(2);
                    product.Specifications = NextLoremIpsum(2);
                    product.SupplyAbility = NextLoremIpsum(2);
                    product.Colour = NextLoremIpsum(1);
                    product.BrandName = NextLoremIpsum(1);

                    new ProductRepository().Save(product);
                }
            }
        }

        private string RandomString(int size)
        {
            StringBuilder builder = new StringBuilder();
            Random random = new Random();
            char ch;
            for (int i = 0; i < size; i++)
            {
                ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65)));
                builder.Append(ch);
            }

            return builder.ToString();
        }

        private int RandomInt()
        {
            Random random = new Random();
            int randomNumber = random.Next(0, 10000);
            return randomNumber;
        }

        #region Latin Generator

        public static string ToFirstCharacterUpperCase(string Input)
        {
            if (string.IsNullOrEmpty(Input))
                return null;
            char[] InputChars = Input.ToCharArray();
            for (int x = 0; x < InputChars.Length; ++x)
            {
                if (InputChars[x] != ' ' && InputChars[x] != '\t')
                {
                    InputChars[x] = char.ToUpper(InputChars[x]);
                    break;
                }
            }
            return new string(InputChars);
        }

        /// <summary> 
        /// Creates a Lorem Ipsum sentence.
        /// </summary> 
        /// 
        /// <param name="NumberOfWords">Number of words for the sentence</param> 
        /// <returns>A string containing Lorem Ipsum text</returns> 
        public static string NextLoremIpsum(int NumberOfWords)
        {
            StringBuilder Builder = new StringBuilder();
            Random rnd = new Random();
            Builder.Append(ToFirstCharacterUpperCase(Words[rnd.Next(Words.Length)]));
            for (int x = 1; x < NumberOfWords; ++x)
            {
                Builder.Append(" ").Append(Words[rnd.Next(Words.Length)]);
            }
            Builder.Append(".");
            return Builder.ToString();
        }

        private static string[] Words = new string[] 
        { 
            "consetetur", "sadipscing", "elitr", "sed", "diam", "nonumy", "eirmod", 
            "tempor", "invidunt", "ut", "labore", "et", "dolore", "magna", "aliquyam", "erat", "sed", "diam", "voluptua",
            "at", "vero", "eos", "et", "accusam", "et", "justo", "duo", "dolores", "et", "ea", "rebum", "stet", "clita", 
            "kasd", "gubergren", "no", "sea", "takimata", "sanctus", "est", "lorem", "ipsum", "dolor", "sit", "amet",
            "lorem", "ipsum", "dolor", "sit", "amet", "consetetur", "sadipscing", "elitr", "sed", "diam", "nonumy", "eirmod", 
            "tempor", "invidunt", "ut", "labore", "et", "dolore", "magna", "aliquyam", "erat", "sed", "diam", "voluptua",
            "at", "vero", "eos", "et", "accusam", "et", "justo", "duo", "dolores", "et", "ea", "rebum", "stet", "clita",
            "kasd", "gubergren", "no", "sea", "takimata", "sanctus", "est", "lorem", "ipsum", "dolor", "sit", "amet", 
            "lorem", "ipsum", "dolor", "sit", "amet", "consetetur", "sadipscing", "elitr", "sed", "diam", "nonumy", "eirmod",
            "tempor", "invidunt", "ut", "labore", "et", "dolore", "magna", "aliquyam", "erat", "sed", "diam", "voluptua",
            "at", "vero", "eos", "et", "accusam", "et", "justo", "duo", "dolores", "et", "ea", "rebum", "stet", "clita", 
            "kasd", "gubergren", "no", "sea", "takimata", "sanctus", "est", "lorem", "ipsum", "dolor", "sit", "amet", "duis",
            "autem", "vel", "eum", "iriure", "dolor", "in", "hendrerit", "in", "vulputate", "velit", "esse", "molestie", 
            "consequat", "vel", "illum", "dolore", "eu", "feugiat", "nulla", "facilisis", "at", "vero", "eros", "et", 
            "accumsan", "et", "iusto", "odio", "dignissim", "qui", "blandit", "praesent", "luptatum", "zzril", "delenit", 
            "augue", "duis", "dolore", "te", "feugait", "nulla", "facilisi", "lorem", "ipsum", "dolor", "sit", "amet", 
            "consectetuer", "adipiscing", "elit", "sed", "diam", "nonummy", "nibh", "euismod", "tincidunt", "ut", "laoreet", 
            "dolore", "magna", "aliquam", "erat", "volutpat", "ut", "wisi", "enim", "ad", "minim", "veniam", "quis", 
            "nostrud", "exerci", "tation", "ullamcorper", "suscipit", "lobortis", "nisl", "ut", "aliquip", "ex", "ea", 
            "commodo", "consequat", "duis", "autem", "vel", "eum", "iriure", "dolor", "in", "hendrerit", "in", "vulputate", 
            "velit", "esse", "molestie", "consequat", "vel", "illum", "dolore", "eu", "feugiat", "nulla", "facilisis", "at", 
            "vero", "eros", "et", "accumsan", "et", "iusto", "odio", "dignissim", "qui", "blandit", "praesent", "luptatum", 
            "zzril", "delenit", "augue", "duis", "dolore", "te", "feugait", "nulla", "facilisi", "nam", "liber", "tempor", 
            "cum", "soluta", "nobis", "eleifend", "option", "congue", "nihil", "imperdiet", "doming", "id", "quod", "mazim", 
            "placerat", "facer", "possim", "assum", "lorem", "ipsum", "dolor", "sit", "amet", "consectetuer", "adipiscing", 
            "elit", "sed", "diam", "nonummy", "nibh", "euismod", "tincidunt", "ut", "laoreet", "dolore", "magna", "aliquam", 
            "erat", "volutpat", "ut", "wisi", "enim", "ad", "minim", "veniam", "quis", "nostrud", "exerci", "tation", 
            "ullamcorper", "suscipit", "lobortis", "nisl", "ut", "aliquip", "ex", "ea", "commodo", "consequat", "duis",
            "autem", "vel", "eum", "iriure", "dolor", "in", "hendrerit", "in", "vulputate", "velit", "esse", "molestie", 
            "consequat", "vel", "illum", "dolore", "eu", "feugiat", "nulla", "facilisis", "at", "vero", "eos", "et", "accusam", 
            "et", "justo", "duo", "dolores", "et", "ea", "rebum", "stet", "clita", "kasd", "gubergren", "no", "sea", 
            "takimata", "sanctus", "est", "lorem", "ipsum", "dolor", "sit", "amet", "lorem", "ipsum", "dolor", "sit", 
            "amet", "consetetur", "sadipscing", "elitr", "sed", "diam", "nonumy", "eirmod", "tempor", "invidunt", "ut",
            "labore", "et", "dolore", "magna", "aliquyam", "erat", "sed", "diam", "voluptua", "at", "vero", "eos", "et", 
            "accusam", "et", "justo", "duo", "dolores", "et", "ea", "rebum", "stet", "clita", "kasd", "gubergren", "no", 
            "sea", "takimata", "sanctus", "est", "lorem", "ipsum", "dolor", "sit", "amet", "lorem", "ipsum", "dolor", "sit",
            "amet", "consetetur", "sadipscing", "elitr", "at", "accusam", "aliquyam", "diam", "diam", "dolore", "dolores", 
            "duo", "eirmod", "eos", "erat", "et", "nonumy", "sed", "tempor", "et", "et", "invidunt", "justo", "labore", 
            "stet", "clita", "ea", "et", "gubergren", "kasd", "magna", "no", "rebum", "sanctus", "sea", "sed", "takimata",
            "ut", "vero", "voluptua", "est", "lorem", "ipsum", "dolor", "sit", "amet", "lorem", "ipsum", "dolor", "sit", 
            "amet", "consetetur", "sadipscing", "elitr", "sed", "diam", "nonumy", "eirmod", "tempor", "invidunt", "ut",
            "labore", "et", "dolore", "magna", "aliquyam", "erat", "consetetur", "sadipscing", "elitr", "sed", "diam", 
            "nonumy", "eirmod", "tempor", "invidunt", "ut", "labore", "et", "dolore", "magna", "aliquyam", "erat", "sed",
            "diam", "voluptua", "at", "vero", "eos", "et", "accusam", "et", "justo", "duo", "dolores", "et", "ea", 
            "rebum", "stet", "clita", "kasd", "gubergren", "no", "sea", "takimata", "sanctus", "est", "lorem", "ipsum" 
        };

        #endregion

    }
}