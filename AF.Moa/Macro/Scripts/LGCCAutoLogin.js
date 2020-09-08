var confirmButton = document.getElementById('loginSubmit');
if (confirmButton != null) {
    document.getElementById('login_userId').value = "";
    document.getElementById('login_userPw').value = "";
    confirmButton.click();
} 