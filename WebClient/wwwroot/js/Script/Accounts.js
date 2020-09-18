var arr = [];

$(document).ready(function () {
    debugger;
    $('#Joblists').DataTable({
        "processing": true,
        "responsive": true,
        "pagination": true,
        "stateSave": true,
        "ajax": {
            url: "/Joblists/LoadJoblist",
            type: "GET",
            dataType: "json",
            dataSrc: "",
        },
        "columns": [
            {
                "data": "id",
                //render: function (data, type, row, meta) {
                //    return meta.row + meta.settings._iDisplayStart + 1;
                //    //return meta.row + 1;
                //}
            },
            { "data": "name" },
            //{
            //    "data": "createData",
            //    'render': function (jsonDate) {
            //        //var date = new Date(jsonDate).toDateString();
            //        //return date;
            //        var date = new Date(jsonDate);
            //        return moment(date).format('DD MMMM YYYY') + '<br> Time : ' + moment(date).format('HH: mm');
            //        //return ("0" + date.getDate()).slice(-2) + '-' + ("0" + (date.getMonth() + 1)).slice(-2) + '-' + date.getFullYear();
            //    }
            //},
            //{
            //    "data": "updateDate",
            //    'render': function (jsonDate) {
            //        //debugger;
            //        //var date = new Date(jsonDate).toDateString();
            //        //return date;
            //        var date = new Date(jsonDate);
            //        if (date.getFullYear() != 0001) {
            //            return moment(date).format('DD MMMM YYYY') + '<br> Time : ' + moment(date).format('HH: mm');
            //            //return ("0" + date.getDate()).slice(-2) + '-' + ("0" + (date.getMonth() + 1)).slice(-2) + '-' + date.getFullYear();
            //        }
            //        return "Not updated yet";
            //    }
            //},
            {
                "sortable": false,
                "render": function (data, type, row) {
                    //console.log(row);
                    $('[data-toggle="tooltip"]').tooltip();
                    return '<button class="btn-circle btn-success" data-placement="left" data-toggle="tooltip" data-animation="false" title="Apply" onclick="return GetById(' + row.id + ')" ><i class="fa fa-lg fa-arrow-circle-right"></i></button>'
                }
            }
        ]
    });
});

function GetById(id) {
    debugger;
    $.ajax({
        url: "/joblists/GetById/",
        data: { id: id }
    }).then((result) => {
        debugger;
        window.location.href = "/login";
        $('#Id').val(result.id);
        $('#Name').val(result.name);
        $('#add').hide();
        $('#update').show();
        $('#myModal').modal('show');
    })
}

function Login() {
    //debugger;
    var validate = new Object();
    validate.Email = $('#Email').val();
    validate.Password = $('#Password').val();
    $.ajax({
        type: 'POST',
        url: "/validate/",
        cache: false,
        dataType: "JSON",
        data: validate
    }).then((result) => {
        //debugger;
        if (result.status == true) {
            if (result.msg == "VerifyCode") {
                window.location.href = "/verify?mail=" + validate.Email;
            } else {
                window.location.href = "/profile";
            }
        } else {
            $.notify({
                // options
                icon: 'fas fa-alarm-clock',
                title: 'Notification',
                message: result.msg,
            }, {
                    // settings
                    element: 'body',
                    type: "danger",
                    allow_dismiss: true,
                    placement: {
                        from: "top",
                        align: "center"
                    },
                    timer: 1000,
                    delay: 5000,
                    animate: {
                        enter: 'animated fadeInDown',
                        exit: 'animated fadeOutUp'
                    },
                    icon_type: 'class',
                    template: '<div data-notify="container" class="col-xs-11 col-sm-3 alert alert-{0}" role="alert">' +
                        '<button type="button" aria-hidden="true" class="close" data-notify="dismiss">×</button>' +
                        '<span data-notify="icon"></span> ' +
                        '<span data-notify="title">{1}</span> ' +
                        '<span data-notify="message">{2}</span>' +
                        '<div class="progress" data-notify="progressbar">' +
                        '<div class="progress-bar progress-bar-{0}" role="progressbar" aria-valuenow="0" aria-valuemin="0" aria-valuemax="100" style="width: 0%;"></div>' +
                        '</div>' +
                        '<a href="{3}" target="{4}" data-notify="url"></a>' +
                        '</div>'
                });
        }
    })
}

function Register() {
    if ($('#confirmPass').val() == $('#Password').val()) {
        debugger;
        var auth = new Object();
        auth.Username = $('#Username').val();
        auth.Email = $('#Email').val();
        auth.Password = $('#Password').val();
        auth.Phone = $('#Phone').val();
        auth.Joblists = $('#joblist').val();
        $.ajax({
            type: 'POST',
            url: "/validate/",
            cache: false,
            dataType: "JSON",
            data: auth
        }).then((result) => {
            debugger;
            if (result.status == true) {
                debugger;
                $.notify({
                    
                    // options
                    icon: 'fas fa-alarm-clock',
                    title: 'Notification',
                    message: result.msg,
                }, {
                        // settings
                        element: 'body',
                        type: "success",
                        allow_dismiss: true,
                        placement: {
                            from: "top",
                            align: "center"
                        },
                        timer: 1000,
                        delay: 5000,
                        animate: {
                            enter: 'animated fadeInDown',
                            exit: 'animated fadeOutUp'
                        },
                        icon_type: 'class',
                        template: '<div data-notify="container" class="col-xs-11 col-sm-3 alert alert-{0}" role="alert">' +
                            '<button type="button" aria-hidden="true" class="close" data-notify="dismiss">×</button>' +
                            '<span data-notify="icon"></span> ' +
                            '<span data-notify="title">{1}</span> ' +
                            '<span data-notify="message">{2}</span>' +
                            '<div class="progress" data-notify="progressbar">' +
                            '<div class="progress-bar progress-bar-{0}" role="progressbar" aria-valuenow="0" aria-valuemin="0" aria-valuemax="100" style="width: 0%;"></div>' +
                            '</div>' +
                            '<a href="{3}" target="{4}" data-notify="url"></a>' +
                            '</div>'
                    });
                window.location.href = "/verify?mail=" + auth.Email;
            } else {
                debugger;
                $.notify({
                    // options
                    icon: 'fas fa-alarm-clock',
                    title: 'Notification',
                    message: result.msg,
                }, {
                        // settings
                        element: 'body',
                        type: "danger",
                        allow_dismiss: true,
                        placement: {
                            from: "top",
                            align: "center"
                        },
                        timer: 1000,
                        delay: 5000,
                        animate: {
                            enter: 'animated fadeInDown',
                            exit: 'animated fadeOutUp'
                        },
                        icon_type: 'class',
                        template: '<div data-notify="container" class="col-xs-11 col-sm-3 alert alert-{0}" role="alert">' +
                            '<button type="button" aria-hidden="true" class="close" data-notify="dismiss">×</button>' +
                            '<span data-notify="icon"></span> ' +
                            '<span data-notify="title">{1}</span> ' +
                            '<span data-notify="message">{2}</span>' +
                            '<div class="progress" data-notify="progressbar">' +
                            '<div class="progress-bar progress-bar-{0}" role="progressbar" aria-valuenow="0" aria-valuemin="0" aria-valuemax="100" style="width: 0%;"></div>' +
                            '</div>' +
                            '<a href="{3}" target="{4}" data-notify="url"></a>' +
                            '</div>'
                    });
            }
        })
    } else {
        debugger
        $.notify({
            // options
            icon: 'fas fa-alarm-clock',
            title: 'Notification',
            message: 'Password Not Same',
        }, {
                // settings
                element: 'body',
                type: "warning",
                allow_dismiss: true,
                placement: {
                    from: "top",
                    align: "center"
                },
                timer: 1000,
                delay: 5000,
                animate: {
                    enter: 'animated fadeInDown',
                    exit: 'animated fadeOutUp'
                },
                icon_type: 'class',
                template: '<div data-notify="container" class="col-xs-11 col-sm-3 alert alert-{0}" role="alert">' +
                    '<button type="button" aria-hidden="true" class="close" data-notify="dismiss">×</button>' +
                    '<span data-notify="icon"></span> ' +
                    '<span data-notify="title">{1}</span> ' +
                    '<span data-notify="message">{2}</span>' +
                    '<div class="progress" data-notify="progressbar">' +
                    '<div class="progress-bar progress-bar-{0}" role="progressbar" aria-valuenow="0" aria-valuemin="0" aria-valuemax="100" style="width: 0%;"></div>' +
                    '</div>' +
                    '<a href="{3}" target="{4}" data-notify="url"></a>' +
                    '</div>'
            });
    }
}

function Verify() {
    debugger;
    const urlParams = new URLSearchParams(window.location.search);
    var validate = new Object();
    validate.Email = urlParams.get('mail');
    validate.VerifyCode = $('#Code').val();
    $.ajax({
        type: 'POST',
        //url: "/verifCode/",
        url: "/validate/",
        cache: false,
        dataType: "JSON",
        data: validate
    }).then((result) => {
        debugger;
        if (result.status == true) {
            window.location.href = "/";
            //Login();
        } else {
            $.notify({
                // options
                icon: 'fas fa-alarm-clock',
                title: 'Notification',
                message: result.msg,
            }, {
                    // settings
                    element: 'body',
                    type: "danger",
                    allow_dismiss: true,
                    placement: {
                        from: "top",
                        align: "center"
                    },
                    timer: 1000,
                    delay: 5000,
                    animate: {
                        enter: 'animated fadeInDown',
                        exit: 'animated fadeOutUp'
                    },
                    icon_type: 'class',
                    template: '<div data-notify="container" class="col-xs-11 col-sm-3 alert alert-{0}" role="alert">' +
                        '<button type="button" aria-hidden="true" class="close" data-notify="dismiss">×</button>' +
                        '<span data-notify="icon"></span> ' +
                        '<span data-notify="title">{1}</span> ' +
                        '<span data-notify="message">{2}</span>' +
                        '<div class="progress" data-notify="progressbar">' +
                        '<div class="progress-bar progress-bar-{0}" role="progressbar" aria-valuenow="0" aria-valuemin="0" aria-valuemax="100" style="width: 0%;"></div>' +
                        '</div>' +
                        '<a href="{3}" target="{4}" data-notify="url"></a>' +
                        '</div>'
                });
        }
    })
}