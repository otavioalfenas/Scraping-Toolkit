
[![Contributors][contributors-shield]][contributors-url]
[![Forks][forks-shield]][forks-url]
[![Issues][issues-shield]][issues-url]
[![MIT License][license-shield]][license-url]



# Scraping-Toolkit

*Read this in other language: [English](README.md), [Portuguese](README.ptbr.md)*

## Overview
The Scrapping-Toolkit is a fast-based structure to capture information within web pages, used to track websites and even extract or insert data on the web pages. It can be widely used to reach to any goal from data-mining to web site monitoring and automated tests.

## Prerequisites
[![HTML Agility Pack][agility-pack-shield]][agility-pack-url] or higher

[![Framework][framework-shield]][framework-url] or higher

[![Framework][framework-core-shield]][framework-core-url] or higher

## How to use

.NET Framework

To install the component you can use the "Install" command or access https://www.nuget.org/packages/Scraping/

```PowerShell
Install-Package Scraping
```

.NET Core

To install the component you can use the "Install" command or access https://www.nuget.org/packages/Scraping.Core/

```PowerShell
Install-Package Scraping.Core
```

To make it use the "load", you must inform the url (FromUrl) and one usage possibility is to let the tool try to identify the screen components.

```C#
public void LoadComponents()
{
	var ret = new HttpRequestFluent(true)
		.FromUrl("https://github.com/otavioalfenas/Scraping-Toolkit")
		.TryGetComponents(Scraping.Enums.TypeComponent.LinkButton| Scraping.Enums.TypeComponent.InputHidden)
		.Load();
}
```

or async method

```C#
public async void LoadComponents()
{
	var ret = await new HttpRequestFluent(true)
		.FromUrl("https://github.com/otavioalfenas/Scraping-Toolkit")
		.TryGetComponents(Scraping.Enums.TypeComponent.LinkButton| Scraping.Enums.TypeComponent.InputHidden)
		.LoadAsync();
}
```

Inside the tool, there are also many extensions that make the parse work easier.

```C#
public void AllTags()
{
	var ret = new HttpRequestFluent(true)
		.FromUrl("https://github.com/otavioalfenas/Scraping-Toolkit")
		.Load();
	var byClassContain = ret.HtmlPage.GetByClassNameContains("Box mb-3 Box--");
	var byClassEquals = ret.HtmlPage.GetByClassNameEquals("Box mb-3 Box--condensed");
	var byId = ret.HtmlPage.GetById("readme");
}
```

## Examples

Below there is an example of all the methods inside the Load. 
The folder "test" contains many examples on Load usage and extensions.
If any doubt or suggestion comes up, you may contact us or open an issue so we can improve the tool together.

```C#
public void LoagPageFull()
{
	var ret = new HttpRequestFluent(true);
	ret.OnLoad += Ret_OnLoad;
	NameValueCollection parameters = new NameValueCollection();
	parameters.Add("Name", "Value");

	ret.FromUrl("https://github.com/otavioalfenas/Scraping-Toolkit")
		.TryGetComponents(Enums.TypeComponent.ComboBox| Enums.TypeComponent.DataGrid| 
						Enums.TypeComponent.Image|Enums.TypeComponent.InputCheckbox|
						Enums.TypeComponent.InputHidden| Enums.TypeComponent.InputText|
						Enums.TypeComponent.LinkButton)
		.RemoveHeader("name")
		.AddHeader("name", "value")
		.KeepAlive(true)
		.WithAccept("Accept")
		.WithAcceptEncoding("Accept-Encoding")
		.WithAcceptLanguage("Accept-Language")
		.WithAutoRedirect(true)
		.WithContentType("ContentType")
		.WithMaxRedirect(2)
		.WithParameters(parameters)
		.WithPreAuthenticate(true)
		.WithReferer("Referer")
		.WithRequestedWith("WithRequestedWidth")
		.WithTimeoutRequest(100)
		.WithUserAgent("User-Agent")
	.Load();

}

private void Ret_OnLoad(object sender, RequestHttpEventArgs e)
{
	e.HtmlPage;
	e.ResponseHttp;
}
```

## Contribution

Below you can contribute to the project as much as you want. Any advice,suggestion or adjust will always be welcomed. 
Here is a step-by-step guide on how to proceed to upload your update.

1. Fork the Project;
2. Create your Feature Branch (`git checkout -b branch/Example`);
3. Commit your updates (`git commit -m 'Message of any updates that were made to the program'`);

Request permission to send your branch.
4. Send to your Branch (`git push --set-upstream origin Example`);
5. Open a Pull Request;

## Licences

Distributed over GNU Licence. See the file `LICENSE` for more information.

## Contact

Otavio Alfenas: [@otavioalfenas](https://br.linkedin.com/in/otavio-alfenas)<br/>
E-mail: otavioalfenas@hotmail.com<br/>

Leandro Klaiber: [@leandroklaiber](https://br.linkedin.com/in/leandroklaiber)<br/>
E-mail: leandroklaiber@gmail.com<br/>

## Acknowledgement

Eduardo Chen - https://www.linkedin.com/in/EduardoChen <br/>
Edgard Yamashita - https://www.linkedin.com/in/eguilherme


[contributors-shield]: https://img.shields.io/github/contributors/otavioalfenas/Scraping-Toolkit.svg?style=flat-square
[contributors-url]: https://github.com/otavioalfenas/Scraping-Toolkit/graphs/contributors
[forks-shield]: https://img.shields.io/github/forks/otavioalfenas/Scraping-Toolkit.svg?style=flat-square
[forks-url]: https://https://github.com/otavioalfenas/Scraping-Toolkit/network/members
[issues-shield]: https://img.shields.io/github/issues/otavioalfenas/Scraping-Toolkit.svg?style=flat-square
[issues-url]: https://github.com/otavioalfenas/Scraping-Toolkit/issues
[license-shield]: https://img.shields.io/github/license/otavioalfenas/Scraping-Toolkit.svg?style=flat-square
[license-url]: https://github.com/otavioalfenas/Scraping-Toolkit/blob/master/LICENSE.txt
[agility-pack-shield]: https://img.shields.io/badge/HtmlAgilityPack-v1.11.18-blue
[agility-pack-url]: https://www.nuget.org/packages/HtmlAgilityPack/1.11.18
[framework-shield]: https://img.shields.io/badge/.net%20Framework-v4.6.1-green
[framework-core-shield]: https://img.shields.io/badge/.net%20Core-v3.1-blue
[framework-url]: https://www.microsoft.com/pt-BR/download/details.aspx?id=49982 
[framework-core-url]: https://dotnet.microsoft.com/download/dotnet-core
