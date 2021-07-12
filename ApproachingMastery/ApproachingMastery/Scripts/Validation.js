function ValidateEmailAddress(text)
{
    var regex = /^([a-zA-Z0-9_\-\.]+)@([a-zA-Z0-9_\-\.]+)\.([a-zA-Z]{2,5})$/;
    var ok = regex.exec(text);
    //alert(ok);
    if (!ok) return false;
    return true;
}


function ValidatePassword(password, confirmPassword, error)
{
    const MIN_PW_LENGTH = 8;

    var hasErrors = false;

    var specialCharacters = /[\ \!\\\"\#\$\%\&\'\(\)\*\,\-\.\/\:\;\<\=\>\?\@\[\\\]\^\_\`\{\|\}\~]/;
    var letterAtBeginning = /^[a-zA-Z].*/;
    var hasUpper = /.*[A-Z].*/;
    var hasNumber = /.*\d.*/;
    var specialCharAtBeginning = new RegExp(/^/ + specialCharacters.source);
    var specialCharCheck = specialCharacters.exec(password);
    var letterAtBeginningCheck = letterAtBeginning.exec(password); 
    var hasUpperCheck = hasUpper.exec(password);
    var hasNumberCheck = hasNumber.exec(password);
    //alert(letterAtBeginningCheck);
    //alert(specialAtStartCheck);
    var check = null;
    if (password.length < MIN_PW_LENGTH)
    {
        error.message = "Password must be at least " + MIN_PW_LENGTH + " characters";
    }
    if (!hasUpperCheck)
    {
        if (error.message !== "") error.message += "\r\n";
        error.message += "Password must contain at least 1 upper case character";
        hasErrors = true;
    }
    if (!letterAtBeginningCheck)
    {
        if (error.message !== "") error.message += "\r\n";
        error.message += "Password must not start with a special character or number";
        hasErrors = true;
    }
    if (!specialCharCheck)
    {
        if (error.message !== "") error.message += "\r\n";
        error.message += "Password must contain at least one special character";
        hasErrors = true;
    }
    if (!hasNumberCheck)
    {
        if (error.message !== "") error.message += "\r\n";
        error.message += "Password must contain at least one number";
        hasErrors = true;
    }
    return !hasErrors;
}

function ValidatePhoneNumber(type, value)
{
    if (type.toUpperCase() === "US")
    {
        expr1 = /^\(\d{3}\)-\d{3}-\d{4}$/;
        expr2 = /^\d{3}-\d{3}-\d{4}$/;
        return expr1.exec(value) || expr2.exec(value);
    }
    return false;
}

function ValidateNotEmpty(input)
{
    return input.trim()!== "";
}

