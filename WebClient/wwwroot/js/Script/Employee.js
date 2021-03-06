﻿var table = null;
var nb = 0;

$(document).ready(function () {
    debugger;
    table = $('#Jobseeker').DataTable({
        "processing": true,
        "responsive": true,
        "pagination": true,
        "stateSave": true,
        "ajax": {
            url: "/employees/LoadJobSeeker",
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
            { "data": "name" },
            { "data": "joblistName" },
            //{ "data": "phone" },
            //{
            //    "data": "createData",
            //    'render': function (jsonDate) {
            //        //debugger;
            //        var date = new Date(jsonDate);
            //        if (date.getFullYear() != 0001) {
            //            return moment(date).format('DD MMMM YYYY') + '<br> Time : ' + moment(date).format('HH:mm');
            //        }
            //        return "Not updated yet";
            //    }
            //},
            {
                "sortable": false,
                "render": function (data, type, row, meta) {
                    $('[data-toggle="tooltip"]').tooltip();
                    return '<button class="btn btn-outline-info btn-circle" data-placement="right" data-toggle="tooltip" data-animation="false" title="Detail" onclick="return GetById(' + meta.row + ')" ><i class="fa fa-lg fa-info"></i></button>'

                }
            }
        ],
      
    });
});



function ClearScreen() {
    debugger;
    $('#Id').val('');
    $('#Name').val('');
    $('#Gender').val('');
    $('#BirthDate').val('');
    $('#Address').val('');
    $('#Religion').val('');
    $('#MaritalStatus').val('');
    $('#Nasionality').val('');
    $('#LastEducation').val('');
    $('#GPA').val('');
    $('#TechnicalSkill').val('');
    $('#Experience').val('');
    $('#Achievement').val('');
        //$('#add').hide();
    $('#update').hide();
    $('#add').show();
}

function GetById(number) {
    debugger;
    //console.log(table.row(number).data());
    var id = table.row(number).data().jobSId;
    $.ajax({
        url: "/employees/GetById/",
        data: { Id: id }
    }).then((result) => {
        debugger;
        $('#Id').append(result.jobSId);
        $('#Name').append(result.name);
        $('#Gender').append(result.gender);
        $('#BirthDate').append(result.birth_Date);
        $('#Address').append(result.address);
        $('#Religion').append(result.religion);
        $('#MaritalStatus').append(result.marital_Status);
        $('#Nasionality').append(result.nationality);
        $('#LastEducation').append(result.last_Education);
        $('#GPA').append(result.gpa);
        $('#TechnicalSkill').append(result.technical_Skill);
        $('#Experience').append(result.experience);
        $('#Achievement').append(result.achievement);
        //$('#add').hide();
        $('#Name').val(result.name);
        $('#Id').val(result.jobSId);
        $('#update').show();
        $('#myModal').modal('show');
    })
}


function Approve() {
    debugger;
    var Acc = new Object();
    Acc.Id = $('#Id').val();
    Acc.Name = $('#Name').val();
    $.ajax({
        type: 'POST',
        url: "employees/Approve/",
        cache: false,
        dataType: "JSON",
        data: Acc
    }).then((result) => {
        //debugger;
        if (result.statusCode == 200) {
            Swal.fire({
                position: 'center',
                icon: 'success',
                title: 'Jobseeker Has Been Approved',
                showConfirmButton: false,
                timer: 1500,
            });
            table.ajax.reload(null, false);
        } else {
            Swal.fire('Error', 'Failed to Approved', 'error');
            ClearScreen();
        }
    })
}

function Reject() {
    debugger;
    var Acc = new Object();
    Acc.Id = $('#Id').val();
    Acc.Name = $('#Name').val();
    $.ajax({
        type: 'POST',
        url: "employees/Reject/",
        cache: false,
        dataType: "JSON",
        data: Acc
    }).then((result) => {
        //debugger;
        if (result.statusCode == 200) {
            Swal.fire({
                position: 'center',
                icon: 'success',
                title: 'Jobseeker Has Been Rejecct',
                showConfirmButton: false,
                timer: 1500,
            });
            table.ajax.reload(null, false);
        } else {
            Swal.fire('Error', 'Failed to Reject', 'error');
            ClearScreen();
        }
    })
}







