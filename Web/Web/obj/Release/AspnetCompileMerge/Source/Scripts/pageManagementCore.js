$(document).ready(function () {
    resizeNarrow();//product home search page requires narrow page 
    bindCategoryLinks();
    bindItemsPerPageLinks();
    bindPageNavigationLinks();
});

$("#Category").change(function () {
    var rk = $('#Category').val();
    selectedCategoryID = rk;
    if (rk == 0) {
        $.get('/Category/GetCategoryFilterAsPartialView', function (data) {
            $('#CategoryFilterContainer').html(data);
            bindCategoryLinks();
            UpdateCategorySponsorBanner();
        })
    }
    else {
        $('#CategoryLoadingImage').removeClass('hiddenimage');

        $.get('/Category/GetCategoryFilterAsPartialView?parentID=' + rk, function (data) {
            $('#CategoryFilterContainer').html(data);
            bindCategoryLinks();
            UpdateCategorySponsorBanner();
        })
    }

    $.get('/Category/GetNavigationBreadcrumbAsPartialView?categoryid=' + rk + '&leadstring=Home%3EProducts%3E', function (data) {
        $('#BreadCrumbNavigationContainer').html(data);
        bindCategoryLinks();
    })

    Search();
});

function bindPageNavigationLinks() {
    $(".pageforward").click(function () {
        if (currentPage != maxPage) {
            currentPage = currentPage + 1;
            Search(currentPage);
        }
    });

    $(".pageback").click(function () {
        if ((currentPage != 0) && (currentPage != 1)) {
            currentPage = currentPage - 1;
            Search(currentPage);
        }
    });
};

$("#SearchButton").click(function () {
    Search();
});

function bindItemsPerPageLinks() {
    $("#Show25").click(function () {
        $("#Show25").removeClass('selected25');
        $("#Show50").removeClass('selected50');
        $("#Show75").removeClass('selected75');
        $("#Show25").addClass('selected25');
        itemsPerPage = 25;
        Search();
    });

    $("#Show50").click(function () {
        $("#Show25").removeClass('selected25');
        $("#Show50").removeClass('selected50');
        $("#Show75").removeClass('selected75');
        $("#Show50").addClass('selected50');
        itemsPerPage = 50;
        Search();
    });

    $("#Show75").click(function () {
        $("#Show25").removeClass('selected25');
        $("#Show50").removeClass('selected50');
        $("#Show75").removeClass('selected75');
        $("#Show75").addClass('selected75');
        itemsPerPage = 75;
        Search();
    });
}

function bindCategoryLinks() {
    $(".categoryselectlink").click(function (event) {
        event.preventDefault();
        event.stopPropagation();

        selectedCategoryID = $(this).attr("href");

        $.get('/Category/GetNavigationBreadcrumbAsPartialView?categoryid=' + encodeURIComponent(selectedCategoryID) + '&leadstring=Home%3EProducts%3E', function (data) {
            $('#BreadCrumbNavigationContainer').html(data);
            bindCategoryLinks();
        })

        Search();
    });

    $(".parentcategorymenuitem").mouseover(function (mainenterevent) {
        var parentID = $(this).attr('id');
        var childMenuId = parentID.replace("ParentMenuItemID", "ChildMenuId");
        $('#' + childMenuId).removeClass('hiddenmenu').addClass('activemenu');

        $('#' + childMenuId).mouseover(function (enterevent) {
            $('#' + childMenuId).addClass('mouseovermenu');
        });

        $('#' + childMenuId).mouseout(function (exitevent) {
            $('#' + childMenuId).removeClass('mouseovermenu');
        });

        $('#' + childMenuId).mouseleave(function (exitevent) {
            $('#' + childMenuId).removeClass('activemenu').addClass("hiddenmenu");
        });
    });

    $(".parentcategorymenuitem").mouseout(function (event) {
        var parentID = $(this).attr('id');
        var childMenuId = parentID.replace("ParentMenuItemID", "ChildMenuId");

        setTimeout(function () {
            var isOpen = $('#' + childMenuId).hasClass('mouseovermenu');
            if (!isOpen) {
                $('#' + childMenuId).removeClass('activemenu').addClass("hiddenmenu");
            }
        }, 100);
    });
}

//function Search(pageNumber) {
//    $('#SearchLoadingImage').removeClass('hiddenimage');

//    var searchText = encodeURIComponent($('#SearchInput').val());
//    var selectedCountryText = encodeURIComponent($('#Country').val());

//    if (pageNumber == null) {
//        currentPage = 1;
//    }

//    //alert('selectedCategoryID=' + selectedCategoryID + 'currentPage=' + currentPage + 'productsPerPage=' + productsPerPage + 'searchtext=' + searchText + 'country=' + selectedCountryText + 'maxpage=' + maxPage);
//    $.get('/product/GetSearchResultsAsPartialView?categoryID=' + encodeURIComponent(selectedCategoryID) + '&searchText=' + searchText + '&country=' + selectedCountryText + '&page=' + currentPage + '&productsPerPage=' + itemsPerPage, function (data) {
//        $('#ResultsContainer').html(data);
//        new bindItemsPerPageLinks();
//        new bindPageNavigationLinks();

//        UpdatePlatinumBanner();
//        UpdateGoldSilverBanner();
//        UpdateCategorySponsorBanner();
//    })
//}

function UpdateCategorySponsorBanner() {
    $('#categoryadvertbanner').removeClass('sponsor-silver-200x275').addClass('sponsor-platinum-200x275');
    $('#category-sponsor-title').html("DHL");
    $('#category-sponsor-title').attr("href", "http://www.dhl.co.za");
}

function UpdatePlatinumBanner() {
    $.get('/FeaturedProduct/GetPlatinumBannerAsPartialView?categoryID=' + encodeURIComponent(selectedCategoryID), function (platdata) {
        $('#PlatinumBannerContainer').html(platdata);
    })
};

function UpdateGoldSilverBanner() {
    $.get('/FeaturedProduct/GetSilverGoldBannerAsPartialView?categoryID=' + encodeURIComponent(selectedCategoryID), function (silvergolddata) {
        $('#GoldSilverBannerContainer').html(silvergolddata);
        resizeWide();//search results page requires wide page
    })
};