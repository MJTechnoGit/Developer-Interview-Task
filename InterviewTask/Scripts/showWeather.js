$(document).ready(function () {


    $(".showWeatherBtn").click(function () {


        event.stopImmediatePropagation();       

        var url = "/Home/GetWeatherByCity/";
        var id = $(this).attr("data-id");       

        $.ajax({
            url: url,
            type: "POST",
            data: { "AreaID": id },
            dataType: "json",
            async: true,
            success: function (data, status, xhr) {

                var response = data;                

                $("#" + id).text("Main: " + response.Title + " : " + response.Description);

                //console.log(response);
                
            },
            error: function (jqXHR, textStatus, errorMessage) {
                // This shouldn't really happen but just incase.

            }
        });

    });

});