using AlarmRegistrationSystem.Models.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AlarmRegistrationSystem.Infrastructure
{
    [HtmlTargetElement("ul", Attributes = "page-model")]
    public class PageLinkTagHelper :TagHelper
    {
        public PagingInfo PageModel { get; set; }
        public string PageView { get; set; } = "waves-effect";
        public string PageClassNeutral { get; set; }
        public string PageClassActual { get; set; } = "active";

        private TagBuilder CreateLink(TagBuilder result, int value, string text, string liClass, string linkClass)
        {
            TagBuilder li = new TagBuilder("li");
            TagBuilder tag = new TagBuilder("a");
            li.Attributes["class"] = liClass;
            tag.Attributes["value"] = (value).ToString();
            tag.Attributes["class"] = linkClass;
            tag.InnerHtml.Append(text);
            li.InnerHtml.AppendHtml(tag);
            result.InnerHtml.AppendHtml(li);
            return tag;
        }

        private void CreateTrafficSymbol(TagBuilder result, int borderPage, string symbol, int actionNumber)
        {
            if (PageModel.CurrentPage == borderPage || PageModel.TotalPages == 0)
            {
                CreateLink(result, PageModel.CurrentPage + actionNumber, symbol, "disabled", "");
            }
            else
            {
                CreateLink(result, PageModel.CurrentPage + actionNumber, symbol, PageView, PageClassNeutral);
            }
        }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            TagBuilder result = new TagBuilder("ul");

            //Create < symbol. If Actual Page is First page then set it disable
            const int firstPage = 1;
            const string previousPageSymbol = "<";
            const int decrementCurrentPage = -1;
            CreateTrafficSymbol(result, firstPage, previousPageSymbol, decrementCurrentPage);


            //If no result generate single page link 
            if (PageModel.TotalPages == 0)
            {
                CreateLink(result, 1, "1", PageClassActual, "");
            }
            else
            {
                int start = 0;
                int end = 0;
                //Generate 5 pages for begining
                if(PageModel.CurrentPage <= 3)
                {
                    start = 1;
                    end = 5;
                }
                //Generate pages for (Current - 2, Current +2)
                else if(PageModel.CurrentPage > 3 && PageModel.CurrentPage < (PageModel.TotalPages - 2))
                {
                    start = PageModel.CurrentPage - 2;
                    end = PageModel.CurrentPage + 2;
                }
                //Generete 5 pages for end
                else
                {
                    start = PageModel.TotalPages - 4;
                    end = PageModel.TotalPages;
                }

                for(int i = start; i <= end; i++)
                {
                    if (i <= PageModel.TotalPages)
                    {
                        if (i == PageModel.CurrentPage)
                        {
                            CreateLink(result, i, i.ToString(), PageClassActual, "");
                        }
                        else
                        {
                            CreateLink(result, i, i.ToString(), PageView, PageClassNeutral);
                        }
                    }
                }
            }

            //Create > symbol. If Actual Page is last page then set it disable
            const string nextPageSymbol = ">";
            const int incrementCurrentPage = 1;
            CreateTrafficSymbol(result, PageModel.TotalPages, nextPageSymbol, incrementCurrentPage);

            output.Content.AppendHtml(result.InnerHtml);
        }
    }
}
