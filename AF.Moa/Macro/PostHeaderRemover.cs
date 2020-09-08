using mshtml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Navigation;

/**
 * 머릿말 제거 코드
 */
namespace AF.Moa.Macro
{
    class PostHeaderRemover : IMacro
    {
        private string[] Headers;

        public PostHeaderRemover()
        {
            Headers = File.ReadAllLines("./Macro/PostHeaders.txt");
        }
        public void OnPageLoaded(IEController controller, NavigationEventArgs e)
        {
            if (!e.Uri.ToString().StartsWith("http://www.af.mil/user/boardList.action?command=view")) return;
            try
            {
                var container = controller.Document.getElementById("divView");
                var src = container.innerHTML;
                foreach (var line in Headers)
                {
                    if (string.IsNullOrEmpty(line)) continue;
                    src = src.Replace(line.Trim(), "");
                }
                controller.Document.getElementById("divView").innerHTML = src;
            }
            catch
            {

            }
            //var @new = controller.Document.documentElement.innerHTML.Replace("<p><span style=\"line-height: 1.6; font-family: 바탕체;\">※ 글을 작성하기 전에 만사마 안내문의 </span><strong><span style=\"line-height: 1.6; font-family: 바탕체;\">만사마 이용 규정</span></strong> <span style=\"line-height: 1.6; font-family: 바탕체;\">과 </span><strong><span style=\"line-height: 1.6; font-family: 바탕체;\">만사마 제재 규정</span></strong> <span style=\"line-height: 1.6; font-family: 바탕체;\">을 </span><strong><span style=\"line-height: 1.6; font-family: 바탕체;\">꼭 확인하시기 바랍니다. </span></strong><span style=\"line-height: 1.6; font-family: 바탕체;\">특히</span> <strong><span style=\"color: rgb(255, 0, 0); line-height: 1.6; font-family: 바탕체;\">자모음의 단독 사용</span><span style=\"line-height: 1.6; font-family: 바탕체;\">, </span><span style=\"color: rgb(255, 0, 0); line-height: 1.6; font-family: 바탕체;\">이모티콘</span><span style=\"line-height: 1.6; font-family: 바탕체;\">및 </span><span style=\"color: rgb(255, 0, 0); line-height: 1.6; font-family: 바탕체;\">땀 표현을 위한 세미콜론</span><span style=\"line-height: 1.6; font-family: 바탕체;\"></span></strong><span style=\"line-height: 1.6; font-family: 바탕체;\">등을 사용할 경우, </span><strong><span style=\"color: rgb(0, 0, 255); line-height: 1.6; font-family: 바탕체; font-size: 11pt;\">미통보 삭제</span><span style=\"color: rgb(0, 0, 255); line-height: 1.6; font-family: 바탕체;\">처리</span></strong> <span style=\"color: rgb(0, 0, 255);\"></span><span style=\"color: rgb(255, 0, 0); line-height: 1.6; font-family: 바탕체;\"></span><span style=\"line-height: 1.6; font-family: 바탕체;\">될 수 있습니다.</span></p>", "");
            //controller.Document.documentElement.innerHTML = @new;
        }
    }
}
