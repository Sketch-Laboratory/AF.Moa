var a = $('.listTb tbody a');
var l = a.length;
var x = document.onkeydown = function (eo) {
    var e = eo ? eo : window.event;
    if (e.ctrlKey != true || e.shiftKey != true) return; // 조합키 체크
    if (e.keyCode == 'x'.charCodeAt() - 32) a[l - 1].click(); // 다음글(x키)
    if (e.keyCode == 'z'.charCodeAt() - 32 && l > 1) a[0].click(); // 이전글(z키)
    if (e.keyCode == 'c'.charCodeAt() - 32) $('#comment').focus(); //덧글창으로
    if (e.keyCode == 13) $('.commentWrite input')[2].click(); //등록버튼
};