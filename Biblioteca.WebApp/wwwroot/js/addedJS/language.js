function GetLanguage(controller,method,language) {
    $.ajax({
        type: "GET",
        url: "/Controllers/" + controller + "/" + method,
        contentType: "application/json; charset=utf-8",
        data: "{lang:" + language + "}",
        dataType: "json",
        success: function (data) {
            //alert(JSON.stringify(data));                  
           
            console.log(data);
        }, //End of AJAX Success function  

        failure: function (data) {
            alert(data.responseText);
        }, 
        error: function (data) {
            alert(data.responseText);
        }  
    });
}