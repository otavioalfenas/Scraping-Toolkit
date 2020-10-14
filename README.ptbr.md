
[![Contributors][contributors-shield]][contributors-url]
[![Forks][forks-shield]][forks-url]
[![Issues][issues-shield]][issues-url]
[![MIT License][license-shield]][license-url]



# Scraping-Toolkit

*Você pode ler também em outro idioma: [English](README.md), [Portuguese](README.ptbr.md)*

## Visão Geral 
O Scraping-Toolkit é uma estrutura rápida de captura de informações de paginas web, usado para rastrear sites e extrair ou inserir dados em suas páginas. Ele pode ser usado para uma ampla variedade de finalidades, desde a mineração de dados até monitoramento e testes automatizados.

## Pré-Requisitos
[![HTML Agility Pack][agility-pack-shield]][agility-pack-url] ou superior

[![Framework][framework-shield]][framework-url] ou superior

[![Framework][framework-core-shield]][framework-core-url] ou superior

## Utilização

.NET Framework

Para instalar o componente você pode usar o comando Install ou acessar https://www.nuget.org/packages/Scraping/

```PowerShell
Install-Package Scraping
```

.NET Core

Para instalar o componente você pode usar o comando Install ou acessar https://www.nuget.org/packages/Scraping.Core/

```PowerShell
Install-Package Scraping.Core
```

Para fazer utilizar o load você deve informar a url (FromUrl) e uma das possibilidade é deixar que a ferramenta tente identificar os componentes da tela.
```C#
public void LoadComponents()
{
	var ret = new HttpRequestFluent(true)
		.FromUrl("https://github.com/otavioalfenas/Scraping-Toolkit")
		.TryGetComponents(Scraping.Enums.TypeComponent.LinkButton| Scraping.Enums.TypeComponent.InputHidden)
		.Load();
}
```
ou utilizando método assíncrono

```C#
public async void LoadComponents()
{
	var ret = await new HttpRequestFluent(true)
		.FromUrl("https://github.com/otavioalfenas/Scraping-Toolkit")
		.TryGetComponents(Scraping.Enums.TypeComponent.LinkButton| Scraping.Enums.TypeComponent.InputHidden)
		.LoadAsync();
}
```


Dentro da ferramenta existe também diversas extensões que facilitam o trabalho do parse.
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

## Exemplos
Abaixo tem um exemplo de todos os métodos do Load.
Dentro da pasta test contém diversos exemplos do Load e extentions.
Se tiver alguma dúvida ou sugestão pode entrar em contato conosco ou abrir uma Issue para melhorarmos em conjunto esta ferramenta.
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


## Contribuição

Você pode contribuir para o com a projeto sempre que quiser, qualquer dica sugestão ou ajuste serão sempre bem vindos.
Abaixo um passo a passo de como deve proceder para subir sua atualização:

1. Fork the Project;
2. Crie sua Feature Branch (`git checkout -b branch/Exemplo`);
3. Commit suas alterações (`git commit -m 'Inserir mensagem da alteração efetuada'`);
4. Solicite permissão para enviar sua branch.
5. Envie para a sua Branch (`git push --set-upstream origin Example`);
6. Abra um Pull Request;

## Licenças

Distribuído sob a licença GNU. Veja o arquivo `LICENSE` para mais informações.

## Contato

Otavio Alfenas: [@otavioalfenas](https://br.linkedin.com/in/otavio-alfenas)<br/>
E-mail: otavioalfenas@hotmail.com<br/>

Leandro Klaiber: [@leandroklaiber](https://br.linkedin.com/in/leandroklaiber)<br/>
E-mail: leandroklaiber@gmail.com<br/>

## Agradecimentos

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

