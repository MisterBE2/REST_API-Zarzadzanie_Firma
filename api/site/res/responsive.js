class Responsive
{
    static placeSpinner(containerId, message)
    {
        let currentBody = $("#"+containerId).html();
        let spinner = '<div class="text-center mt-3"><div class="spinner-border" role="status"><span class="sr-only">Loading...</span></div><p>'+message+'</p></div>';

        $("#"+containerId).html(spinner);
        return currentBody;
    }

    static redInputField(inputId, message)
    {
        let small = '<small id="'+inputId+'small" class="form-text text-muted">'+message+'</small>';

        if($("#"+inputId+"small").length == 0)
        {
            $("#"+inputId).after(small);
            $("#"+inputId).addClass("border-danger");
        }
    }
   
    static clearInputField(inputId)
    {
        $('#'+inputId+"small").remove();
        $("#"+inputId).removeClass("border-danger");
    }

    static putAlertAfter(elemId, message)
    {
        let alert = "<div id='alert' class='alert alert-danger m-1 align-self-center' role='alert' id='"+elemId+"alert'>"+message+"</div>";
        Responsive.clearAlert(elemId);
        $('#'+elemId).append(alert);
    }

    static putAlertBefore(elemId, message)
    {
        let alert = "<div id='alert' class='alert alert-danger m-1 align-self-center' role='alert' id='"+elemId+"alert'>"+message+"</div>";
        Responsive.clearAlert(elemId);
        $('#'+elemId).prepend(alert);
    }

    static putAlert(elemId, message)
    {
        let alert = "<div id='alert' class='alert alert-danger m-1 align-self-center' role='alert' id='"+elemId+"alert'>"+message+"</div>";
        Responsive.clearAlert(elemId);
        $('#'+elemId).html(alert);
    }

    static clearAlert(elemId)
    {
        $("#"+elemId+"alert").remove();
    }
}