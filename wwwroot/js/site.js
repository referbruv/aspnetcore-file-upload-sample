$(document).ready(function () {
    $(".form")
        .submit(function (event) {
            debugger;
            event.preventDefault();

            // fetch the form object
            $f = $(event.currentTarget);

            // check if form is valid
            if ($f.valid()) {
                $("div.loader").show();

                // fetch the action and method
                var url = $f.attr("action");
                var method = $f.attr("method");

                if (method.toUpperCase() === "POST") {

                    // prepare the FORM data to POST
                    var data = new FormData(this);

                    // ajax POST
                    $.ajax({
                        url: url,
                        method: "POST",
                        data: data,
                        processData: false,
                        contentType: false,
                        success: handleResponse,
                        error: handleError,
                        complete: function (jqXHR, status) {
                            console.log(jqXHR);
                            console.log(status);
                            $f.trigger('reset');
                        }
                    });
                }
            }
        });

    function handleResponse(res) {
        debugger;
        $("div.loader").hide();

        // check if isSuccess from Response
        // is False or Not set
        if (!res.isSuccess) {
            debugger;
            // handle unsuccessful scenario
        } else {
            debugger;
            // handle successful scenario
            window.location = res.redirectTo;
        }
    }

    function handleError(err) {
        $("div.loader").hide();
        if (xhr.responseText)
            showErrorMessage(xhr.responseText);
        else
            showErrorMessage("Some unknown error has occured. Please try again later.");
    }

    function showErrorMessage(message) {
        debugger;
        // show a popup logic or an alert logic
        var popup = $('#errorAlert');
        popup.removeClass('d-none');
        setTimeout(() => {
            popup.addClass('d-none');
        }, 5000);
    }

    function showSuccessMessage(message) {
        debugger;
        // show a popup logic or an alert logic
        var popup = $('#successAlert');
        popup.text(message);
        popup.removeClass('d-none');
        setTimeout(() => {
            popup.addClass('d-none');
        }, 5000);
    }
});