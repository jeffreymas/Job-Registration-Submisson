﻿var table = null;

$(document).ready(function () {
    table = $('#Jobseeker').DataTable({
        "processing": true,
        "responsive": true,
        "pagination": true,
        "stateSave": true,
        "ajax": {
            url: "/employees/loademp",
            type: "GET",
            dataType: "json",
            dataSrc: "",
        },
        "columns": [
            {
                "data": "jobSId",
                render: function (data, type, row, meta) {
                    //console.log(row);
                    return meta.row + meta.settings._iDisplayStart + 1;
                }
            },
            { "data": "username" },
            { "data": "Joblist" },
            //{ "data": "phone" },
            {
                "data": "createData",
                'render': function (jsonDate) {
                    //debugger;
                    var date = new Date(jsonDate);
                    if (date.getFullYear() != 0001) {
                        return moment(date).format('DD MMMM YYYY') + '<br> Time : ' + moment(date).format('HH:mm');
                    }
                    return "Not updated yet";
                }
            },
            {
                "sortable": false,
                "render": function (data, type, row, meta) {
                    $('[data-toggle="tooltip"]').tooltip();
                    return '<button class="btn btn-outline-info btn-circle" data-placement="left" data-toggle="tooltip" data-animation="false" title="Detail" onclick="return GetById(' + meta.row + ')" ><i class="fa fa-lg fa-info"></i></button>'
                        + '&nbsp;'
                        + '<button class="btn btn-outline-danger btn-circle" data-placement="right" data-toggle="tooltip" data-animation="false" title="Delete" onclick="return Delete(' + meta.row + ')" ><i class="fa fa-lg fa-times"></i></button>'
                }
            }
        ]
    });
});

function ClearScreen() {
    $('#Id').val('');
    $('#Name').val('');
    $('#update').hide();
    $('#add').show();
}

function GetById(number) {
    //debugger;
    //console.log(table.row(number).data());
    var id = table.row(number).data().empId;
    $.ajax({
        url: "/employees/GetById/",
        data: { Id: id }
    }).then((result) => {
        //debugger;
        $('#Id').append(result.empId);
        $('#Name').append(result.username);
        $('#Address').append(result.address);
        $('#Phone').append(result.phone);

        var date = new Date(result.createData);
        $('#HireDate').append(moment(date).format('DD MMMM YYYY'));
        $('#HireTime').append(moment(date).format('LTS'))
        //$('#add').hide();
        $('#update').show();
        $('#myModal').modal('show');
    })
}

function Delete(number) {
    var id = table.row(number).data().empId;
    Swal.fire({
        title: 'Are you sure?',
        text: "You won't be able to revert this!",
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Yes, delete it!',
    }).then((resultSwal) => {
        if (resultSwal.value) {
            //debugger;
            $.ajax({
                url: "/employees/Delete/",
                data: { Id: id }
            }).then((result) => {
                //debugger;
                if (result.statusCode == 200) {
                    //debugger;
                    Swal.fire({
                        position: 'center',
                        icon: 'success',
                        title: 'Delete Successfully',
                        showConfirmButton: false,
                        timer: 1500,
                    });
                    table.ajax.reload(null, false);
                } else {
                    Swal.fire('Error', 'Failed to Delete', 'error');
                    ClearScreen();
                }
            })
        };
    });
}