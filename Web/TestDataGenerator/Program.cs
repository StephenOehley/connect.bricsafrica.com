using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using TestDataGenerator.Repository;
//using TestDataGenerator.Models;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Windows.Controls;
using System.Windows.Media;
using System.Threading;
using AzureHelper.Authentication;
using AzureHelper.Security;
using BricsWeb.Repository;
using BricsWeb.Models;

namespace TestDataGenerator
{
    class Program
    {
        static CompanyRepository compRep;
        static ProductRepository proRep;
        static PhotoRepository photRep;
        static CategoryRepository catRep;
        static AzureUserRepository userRep;
        static AzureRoleRepository roleRep;
        static List<string> Cats = new List<string>();

        static void Main(string[] args)
        {
            BuyerRequestRepository brr = new BuyerRequestRepository();
            brr.ResetOrInitialise();
            
            CompanySubscriptionRepository csr = new CompanySubscriptionRepository();
            csr.Initialise();

            TransactionRepository tr = new TransactionRepository();
            tr.Initialise();

            roleRep = new AzureRoleRepository();
            roleRep.Initialise();

            //User Initialization
            userRep = new AzureUserRepository();
            userRep.Initialise();

            userRep.Save(au);

            string UID = Guid.NewGuid().ToString();
           
            roleRep.Save(arm);

            //Category Initialization
            
            //User Role Initialization
            //roleRep = new AzureRoleRepository();
            //roleRep.ResetOrInitialise();

            //Company Initialization
            compRep = new CompanyRepository();
            compRep.ResetOrInitialise();

            //Product Initialization
            proRep = new ProductRepository();
            proRep.ResetOrInitialise();

            //Photo Initialization
            photRep = new PhotoRepository();
            

            CreateTestData();

        }

        static string GetRandomCategory()
        {
            Random rnd = new Random();
            int p = rnd.Next(0, Cats.Count());

            return Cats[p];
        }

        static byte[] CreateImage(string Name)
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

        private static PngBitmapEncoder getImageFromControl(Control controlToConvert)
        {

            // save current canvas transform

            Transform transform = controlToConvert.LayoutTransform;
            // get size of control
            Size sizeOfControl = new Size(120, 120);//controlToConvert.ActualWidth, controlToConvert.ActualHeight);
            // measure and arrange the control
            controlToConvert.Measure(sizeOfControl);
            // arrange the surface
            controlToConvert.Arrange(new Rect(sizeOfControl));
            // craete and render surface and push bitmap to it
            RenderTargetBitmap renderBitmap = new RenderTargetBitmap((Int32)sizeOfControl.Width, (Int32)sizeOfControl.Height, 96d, 96d, PixelFormats.Pbgra32);
            // now render surface to bitmap
            renderBitmap.Render(controlToConvert);
            // encode png data
            PngBitmapEncoder pngEncoder = new PngBitmapEncoder();
            // puch rendered bitmap into it
            pngEncoder.Frames.Add(BitmapFrame.Create(renderBitmap));
            // return encoder
            return pngEncoder;

        }

        private static byte[] ImageThread(string s)
        {
            return CreateImage(s);
        }

        private static string RandomString(int size)
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
