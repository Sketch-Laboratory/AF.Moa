var n=$('.no img');for(var i=0;i<n.length;i++)var x=n[i].parentElement.parentElement.outerText="";
var c=$('td a');for(var i=0;i<(c.length>9?9:c.length);i++)var x=c[i].innerHTML="<font style='color:red'><b>"+(i+1)+".</b></font> "+c[i].innerHTML;
var x=document.onkeydown=function(eo){var e=eo?eo:window.event;
    if(e.ctrlKey!=true||e.shiftKey!=true)return; // 조합키 체크
    var kc=e.keyCode-49;if(0<=kc&&kc<10)c[kc].click(); //게시물로 이동
};