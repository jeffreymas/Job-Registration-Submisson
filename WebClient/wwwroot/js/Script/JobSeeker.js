var arrList = [];


//function Save() {
//    //debugger;
//    var Jobs = new Object();
//    Jobs.jobSId = 0;
//    Jobs.username = $('#Name').val();
//    Jobs.address = $('#Address').val();
//    Jobs.birth_Date = $('#BirthDate').val();
//    Jobs.nationality = $('#Nasionality').val();
//    Jobs.marital_Status = $('#MaritalStatus').val();
//    Jobs.Gender = $('#Gender').val();
//    Jobs.religion = $('#Religion').val();
//    Jobs.last_Education = $('#LastEducation').val();
//    Jobs.gpa = $('#GPA').val();
//    Jobs.technical_Skill = $('#TechnicalSkill').val();
//    Jobs.experience = $('#Experience').val();
//    Jobs.achievement = $('#Achievement').val();
//    $.ajax({
//        type: 'POST',
//        url: "/DashBoard/InsertOrUpdate/",
//        cache: false,
//        dataType: "JSON",
//        data: Jobs
//    }).then((result) => {
//        //debugger;
//        if (result.statusCode == 200) {
//            Swal.fire({
//                position: 'center',
//                icon: 'success',
//                title: 'Data inserted Successfully',
//                showConfirmButton: false,
//                timer: 1500,
//            })
//            table.ajax.reload(null, false);
//        } else {
//            Swal.fire('Error', 'Failed to Input', 'error');
//            ClearScreen();
//        }
//    })
//}

function Update() {
    debugger;
    var Jobs = new Object();
    Jobs.jobSId = $('#Id').val();
    Jobs.name = $('#Name').val();
    Jobs.address = $('#Address').val();
    Jobs.birth_Date = $('#BirthDate').val();
    Jobs.nationality = $('#Nationality').val();
    Jobs.marital_Status = $('#MaritalStatus').val();
    Jobs.Gender = $('#Gender').val();
    Jobs.religion = $('#Religion').val();
    Jobs.last_Education = $('#LastEducation').val();
    Jobs.gpa = $('#GPA').val();
    Jobs.technical_Skill = $('#TechnicalSkill').val();
    Jobs.experience = $('#Experience').val();
    Jobs.achievement = $('#Achievement').val();
    //Jobs.joblistId = $('#JoblistOption').val();
    $.ajax({
        type: 'POST',
        url: "/DashBoard/InsertOrUpdate/",
        cache: false,
        dataType: "JSON",
        data: Jobs
    }).then((result) => {
        debugger;
        if (result.statusCode == 200) {
            Swal.fire({
                position: 'center',
                icon: 'success',
                title: 'Your Submission Has Been Sent',
                showConfirmButton: false,
                timer: 1500,
            });
            table.ajax.reload(null, false);
        } else {
            Swal.fire('Error', 'Failed to Input', 'error');
            ClearScreen();
        }
    })
}


//function Loadlist(element) {
//    //debugger;
//    if (arrList.length === 0) {
//        $.ajax({
//            type: "Get",
//            url: "/joblists/LoadJoblist",
//            success: function (data) {
//                arrList = data;
//                renderList(element);
//            }
//        });
//    }
//    else {
//        renderList(element);
//    }
//}

//function renderList(element) {
//    var $option = $(element);
//    $option.empty();
//    $option.append($('<option/>').val('0').text('Select List').hide());
//    $.each(arrList, function (i, val) {
//        $option.append($('<option/>').val(val.id).text(val.name))
//    });
//}

//Loadlist($('#JoblistOption'))