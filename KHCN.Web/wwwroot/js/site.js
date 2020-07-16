//$('.main-header').addClass('border-bottom-0');

//$('body').addClass('text-sm');

//$('.main-header').addClass('text-sm');

//$('.nav-sidebar').addClass('text-sm');

//$('.main-footer').addClass('text-sm');

//$('.nav-sidebar').addClass('nav-flat');

//$('.nav-sidebar').addClass('nav-compact');

//$('.nav-sidebar').addClass('nav-child-indent');

//$('.main-sidebar').addClass('sidebar-no-expand');

//$('.brand-link').addClass('text-sm');

//$('.main-sidebar').addClass('sidebar-light-olive');

$('.brand-link').addClass('bg-info');

$('.brand-link').attr("style", "height: 57px");

$("ul.nav.menu li a.nav-link").on('click', function (e) {
    $("a.active").removeClass('active');
    $(this).addClass('active');
});

$(function () {
    var selector = 'ul.nav li';
    var url = window.location.href;
    var arrUrl = url.split('/');
    var url2 = url.replace(arrUrl[arrUrl.length - 1], "Index");
    $(selector).each(function () {
        if ($(this).find('a.nav-link').attr('href') === url || $(this).find('a.nav-link').attr('href') === url2) {
            $(this).find('a.nav-link').removeClass('active').addClass('active');
            var parent = $(this).parent();
            if (parent.hasClass("has-treeview")) {
                parent.removeClass('menu-open').addClass('menu-open');
            }
            else {
                parent = parent.parent();
                if (parent.hasClass("has-treeview")) {
                    parent.removeClass('menu-open').addClass('menu-open');
                }
            }
        }
    });
});

function MsgSuccess(message) {
    var obj = $.alert({
        icon: 'fa fa-spinner fa-spin',
        type: 'green',
        title: 'Thông báo',
        content: message,
    })
    setTimeout(function () {
        obj.close();
    }, 3000); 
};

function MsgError(message) {
    var obj = $.alert({
        icon: 'fa fa-spinner fa-spin',
        type: 'red',
        title: 'Thông báo',
        content: message,
    })
    setTimeout(function () {
        obj.close();
    }, 3000); 
};

function MsgWarning(message) {
    var obj = $.alert({
        icon: 'fa fa-spinner fa-spin',
        type: 'orange',
        title: 'Thông báo',
        content: message,
    })
    setTimeout(function () {
        obj.close();
    }, 3000); 
};