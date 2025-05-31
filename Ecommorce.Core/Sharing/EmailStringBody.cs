using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace Ecommorce.Core.Sharing
{
    public class EmailStringBody
    {
        public static string Send(string email, string token, string component, string message)
        {
            string encodeToken = Uri.EscapeDataString(token);
            string encodedEmail = HtmlEncoder.Default.Encode(email);
            string encodedMessage = HtmlEncoder.Default.Encode(message);

            return $@"
<html>
<head>
    <style>
        .button {{
            border: none;
            border-radius: 10px;
            padding: 15px 30px;
            color: #fff;
            display: inline-block;
            background: linear-gradient(45deg, #ff7e5f, #feb47b);
            cursor: pointer;
            text-decoration: none;
            box-shadow: 0 4px 15px rgba(0, 0, 0, 0.2);
            transition: all 0.3s ease;
            font-size: 16px;
            font-weight: bold;
            font-family: 'Arial', sans-serif;
        }}
    </style>
</head>
<body>
    <h1>{encodedMessage}</h1>
    <hr>
    <br>
    <a class=""button"" href=""http://localhost:4200/account/{component}?email={encodedEmail}&code={encodeToken}"">Click here</a>
</body>
</html>";
        }
    }
    }
