"use strict";
app.controller('RTFController', ['$controller', '$scope', 'MDSOnlineService', function ($controller, $scope, service) {

    angular.extend(this, $controller('BaseController', { $scope: $scope }));

    $scope.TipoEvidenciaEnum = TipoEvidenciaEnum;

    $scope.testes = [];

    $scope.testeSelecionado = {};

    $scope.edicaoHabilitada = false;

    $scope.tipoEvidenciaSelecionado = TipoEvidenciaEnum.SUCESSO;

    $scope.sortableOptions = { 'ui-preserve-size': true, handle: '.sortable-handle', cancel: ".not-sortable" };

    $scope.testesRemovidos = [];

    $scope.init = function () {
        carregarTestes();
        initAtalhosTeclado();
    };

    var carregarTestes = function () {
        service.obterRTF($scope.chamado)
            .then(function (response) {
                $scope.testes = response.data.Testes;
            },
            $scope.erroInsesperado);
    };

    var initAtalhosTeclado = function () {
        Mousetrap.bind(['command+s', 'ctrl+s'], function (e) {
            $scope.salvaRTF();
            return false;
        });

        Mousetrap.bind(['ctrl+z'], function (e) {
            var tagName = e.target.nodeName.toLowerCase();
            var isEditorMedium = e.target.hasAttribute('medium-editor');
            if (tagName === 'textarea' || tagName === 'input' || isEditorMedium) return true;
            $scope.desfazerUltimaExclusao();            
        });
    };

    $scope.selecionarTeste = function (teste) {
        $scope.testeSelecionado = teste;
    };    

    $scope.adicionarEvidencia = function () {
        $scope.testeSelecionado.Evidencias = $scope.testeSelecionado.Evidencias || [];
        $scope.testeSelecionado.Evidencias.push({
            TipoEvidenciaID: TipoEvidenciaEnum.SUCESSO,
            GuidImagem: '',
            Descricao: ''
        });
    };

    $scope.selecionarTipoEvidencia = function (tipo) {
        $scope.tipoEvidenciaSelecionado = +tipo;
    };

    $scope.adicionarNovoTesteFuncional = function () {
        $scope.testes.push({
            Sequencia: ($scope.testes.length || 0) + 1,
            StatusExecucaoHomologacaoID: '' + StatusTesteFuncionalEnum.NAO_TESTADO,
            Ordem: $scope.testes.length + 1,
            Evidencias: []
        });
        $scope.enableTooltips();
    };

    $scope.removerTesteFuncional = function (teste, $index) {
        $scope.testesRemovidos.push({ index: $index, teste: teste });
        $scope.testes.splice($scope.testes.indexOf(teste), 1);
        $scope.enableTooltips();
    };

    $scope.desfazerUltimaExclusao = function () {
        if ($scope.testesRemovidos.length === 0) return;
        var ultimoTesteExcluido = $scope.testesRemovidos.pop();
        $scope.testes.splice(ultimoTesteExcluido.index, 0, ultimoTesteExcluido.teste);
        $scope.enableTooltips();
        $scope.$apply();
    };

    $scope.obterClasseCelulaVerificacao = function (verificacao) {
        switch (+verificacao) {
            case StatusTesteFuncionalEnum.OK:
                return "teste-ok";
            case StatusTesteFuncionalEnum.NOK:
                return "teste-nok";
            case StatusTesteFuncionalEnum.NAO_TESTADO:
                return "teste-nao-testado";
        }
    };

    $scope.habilitarDesabilitarEdicao = function () {
        $scope.edicaoHabilitada = !$scope.edicaoHabilitada;
    };

    $scope.salvarRTF = function () {
        if (!$scope.edicaoHabilitada) return;

        $scope.confirmar("Confirmação", "Deseja salvar suas alterações?", function (confirm) {
            if (confirm) {
                service.salvarRTF({ Chamado: $scope.chamado, Testes: $scope.testes })
                    .then(function (response) {
                        $scope.notify('Documento salvo com sucesso!', 'success');
                    },
                    function (response) {
                        if (response.data.camposComErros && response.data.camposComErros.length > 0) {
                            $scope.exibirMensagemCamposComErros(response.data.camposComErros);
                        }
                        else {
                            $scope.erroInsesperado();
                        }
                    });
            }
        });
    };

    $scope.init();
}]);