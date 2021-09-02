using System.Globalization;
using System.IO;
using Microsoft.Extensions.Configuration;
using Xunit;
using TechTalk.SpecFlow;
using FluentAssertions;
using ConversorDistancias.Testes.PageObjects;

namespace ConversorDistancias.Testes
{
    [Binding]
    public sealed class ConvDistanciasStepDefinition
    {
        private readonly IConfiguration _configuration;
        private double _distanciaMilhas;
        private double _resultadoKm;

        public ConvDistanciasStepDefinition()
        {
            _configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile($"appsettings.json")
                .AddEnvironmentVariables().Build();

            CultureInfo.DefaultThreadCurrentCulture = new ("pt-BR");
            CultureInfo.DefaultThreadCurrentUICulture = new ("pt-BR");
        }

        [Given("que o valor da distância é de (.*) milha\\(s\\)")]
        public void PreencherDistanciaMilhas(double distanciaMilhas)
        {
            _distanciaMilhas = distanciaMilhas;
        }

        [When("eu solicitar a conversão desta distância")]
        public void ProcessarConversao()
        {
            var conversor = new TelaConversaoDistancias(_configuration);

            conversor.CarregarPagina();
            conversor.PreencherDistanciaMilhas(_distanciaMilhas);
            conversor.ProcessarConversao();
            _resultadoKm = conversor.ObterDistanciaKm();
            conversor.Fechar();
        }

        [Then("o resultado será (.*) Km")]
        public void ValidarResultadoKm(double distanciaKm)
        {
            _resultadoKm.Should().Be(distanciaKm, " *** provável problema de arredondamento *** ");
        }
    }
}