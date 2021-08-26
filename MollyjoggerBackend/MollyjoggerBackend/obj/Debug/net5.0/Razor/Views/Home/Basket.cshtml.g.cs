#pragma checksum "C:\Users\User\Desktop\MollyjoggerEndProject\MollyjoggerBackend\MollyjoggerBackend\Views\Home\Basket.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "2ebce29d487e6c1e22681ead11565722db6d688b"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Home_Basket), @"mvc.1.0.view", @"/Views/Home/Basket.cshtml")]
namespace AspNetCore
{
    #line hidden
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
#nullable restore
#line 1 "C:\Users\User\Desktop\MollyjoggerEndProject\MollyjoggerBackend\MollyjoggerBackend\Views\_ViewImports.cshtml"
using MollyjoggerBackend;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "C:\Users\User\Desktop\MollyjoggerEndProject\MollyjoggerBackend\MollyjoggerBackend\Views\_ViewImports.cshtml"
using MollyjoggerBackend.Models;

#line default
#line hidden
#nullable disable
#nullable restore
#line 3 "C:\Users\User\Desktop\MollyjoggerEndProject\MollyjoggerBackend\MollyjoggerBackend\Views\_ViewImports.cshtml"
using MollyjoggerBackend.ViewModels;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"2ebce29d487e6c1e22681ead11565722db6d688b", @"/Views/Home/Basket.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"78e172d4ea340a48a42f7a0f8001523156e7d896", @"/Views/_ViewImports.cshtml")]
    public class Views_Home_Basket : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<dynamic>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            WriteLiteral("\r\n");
#nullable restore
#line 2 "C:\Users\User\Desktop\MollyjoggerEndProject\MollyjoggerBackend\MollyjoggerBackend\Views\Home\Basket.cshtml"
  
    ViewData["Title"] = "Basket";
    List<BasketViewModel> basketViewModels = Model;


#line default
#line hidden
#nullable disable
            WriteLiteral(@"
<table class=""table table-dark"">
    <thead>
        <tr>

            <th scope=""col"">Product Name</th>
            <th scope=""col"">Product Price</th>
            <th scope=""col"">Product Count</th>
            <th scope=""col"">Total</th>
        </tr>
    </thead>
    <tbody>
");
#nullable restore
#line 19 "C:\Users\User\Desktop\MollyjoggerEndProject\MollyjoggerBackend\MollyjoggerBackend\Views\Home\Basket.cshtml"
         foreach (var item in basketViewModels)
        {

#line default
#line hidden
#nullable disable
            WriteLiteral("        <tr>\r\n\r\n            <td>");
#nullable restore
#line 23 "C:\Users\User\Desktop\MollyjoggerEndProject\MollyjoggerBackend\MollyjoggerBackend\Views\Home\Basket.cshtml"
           Write(item.ProductName);

#line default
#line hidden
#nullable disable
            WriteLiteral("</td>\r\n            <td style=\"display: none;\" id=\"hideprice\">");
#nullable restore
#line 24 "C:\Users\User\Desktop\MollyjoggerEndProject\MollyjoggerBackend\MollyjoggerBackend\Views\Home\Basket.cshtml"
                                                 Write(item.Price);

#line default
#line hidden
#nullable disable
            WriteLiteral("</td>\r\n            <td id=\"price\">Price: ");
#nullable restore
#line 25 "C:\Users\User\Desktop\MollyjoggerEndProject\MollyjoggerBackend\MollyjoggerBackend\Views\Home\Basket.cshtml"
                             Write(item.Price);

#line default
#line hidden
#nullable disable
            WriteLiteral("</td>\r\n            <td>\r\n                <div id=\"plus\" onclick=\"pluss()\">+</div> <div id=\"count\">");
#nullable restore
#line 27 "C:\Users\User\Desktop\MollyjoggerEndProject\MollyjoggerBackend\MollyjoggerBackend\Views\Home\Basket.cshtml"
                                                                    Write(item.Count);

#line default
#line hidden
#nullable disable
            WriteLiteral("</div><div onclick=\"minuss()\" id=\"minus\">-</div>\r\n            </td>\r\n            <td id=\"total\">Total : </td>\r\n            <td class=\"temizle\"></td>\r\n            <td class=\"temizle\"></td>\r\n        </tr>\r\n");
#nullable restore
#line 33 "C:\Users\User\Desktop\MollyjoggerEndProject\MollyjoggerBackend\MollyjoggerBackend\Views\Home\Basket.cshtml"
       
        }

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n    </tbody>\r\n</table>");
        }
        #pragma warning restore 1998
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.ViewFeatures.IModelExpressionProvider ModelExpressionProvider { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IUrlHelper Url { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IViewComponentHelper Component { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IJsonHelper Json { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<dynamic> Html { get; private set; }
    }
}
#pragma warning restore 1591
