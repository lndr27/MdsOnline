"use strict";
app.controller('RTFController', ['$controller', '$scope', 'MDSOnlineService', 'FileUploadService', function ($controller, $scope, service, fileUploadService) {

    // TODO: FIX Botao Proximo Evidencia

    //#region Init +
    angular.extend(this, $controller('BaseController', { $scope: $scope }));

    $scope.TipoEvidenciaEnum = TipoEvidenciaEnum;

    $scope.testes = [];

    $scope.testeSelecionado = null;

    $scope.evidenciaSelecionada = null;

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
            $scope.salvarRTF();
            return false;
        });

        Mousetrap.bind(['ctrl+z'], function (e) {
            var tagName = e.target.nodeName.toLowerCase();
            var isEditorMedium = e.target.hasAttribute('medium-editor');
            if (tagName === 'textarea' || tagName === 'input' || isEditorMedium) return true;
            $scope.desfazerUltimaExclusao();            
        });
    };    
    //#endregion

    //#region Testes +
    $scope.adicionarNovoTesteFuncional = function () {
        $scope.testes.push({
            Sequencia: ($scope.testes.length || 0) + 1,
            StatusExecucaoHomologacaoID: '' + StatusTesteFuncionalEnum.NAO_TESTADO,
            Ordem: $scope.testes.length + 1,
            Evidencias: [],
            Erros: []
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
    
    //#endregion

    // #region Evidencias +
    $scope.exibirLinkEvidencias = function (teste) {
        return !_.isEmpty(teste.Evidencias) || !_.isEmpty(teste.Erros);
    };

    $scope.abrirModalEvidencias = function (teste) {

        if (!$scope.exibirLinkEvidencias(teste)) return;

        $scope.testeSelecionado = teste;
        var nomePropriedade = $scope.tipoEvidenciaSelecionado === TipoEvidenciaEnum.SUCESSO ? 'Evidencias' : 'Erros';
        if (!_.isEmpty($scope.testeSelecionado[nomePropriedade])) {
            $scope.evidenciaSelecionada = $scope.testeSelecionado[nomePropriedade][0];
        }
        else {
            $scope.evidenciaSelecionada = null;
        }
        $scope.abrirModal('#modalEvidencias');
    };

    $scope.adicionarEvidencia = function () {
        $scope.tipoEvidenciaSelecionado === TipoEvidenciaEnum.SUCESSO ?
            adicionarEvidencia('Evidencias') :
            adicionarEvidencia('Erros');
    };

    var adicionarEvidencia = function (nomePropriedadeModel) {
        $scope.testeSelecionado[nomePropriedadeModel] = $scope.testeSelecionado[nomePropriedadeModel] || [];
        var novaEvidencia = {
            TipoEvidenciaID: $scope.tipoEvidenciaSelecionado,
            GuidImagem: '',
            Descricao: ''
        };
        $scope.testeSelecionado[nomePropriedadeModel].push(novaEvidencia);
        $scope.evidenciaSelecionada = novaEvidencia;
    };

    $scope.apagarEvidencia = function () {
        $scope.tipoEvidenciaSelecionado === TipoEvidenciaEnum.SUCESSO ?
            apagarEvidencia('Evidencias') :
            apagarEvidencia('Erros');
    };

    var apagarEvidencia = function (nomePropriedadeModel) {
        var index = $scope.obterIndexEvidenciaSelecionada();
        $scope.testeSelecionado[nomePropriedadeModel].splice(index, 1);
        if (!_.isEmpty($scope.testeSelecionado[nomePropriedadeModel])) {
            if (index > 0) {
                $scope.evidenciaSelecionada = $scope.testeSelecionado[nomePropriedadeModel][index - 1];
            }
            else {
                $scope.evidenciaSelecionada = $scope.testeSelecionado[nomePropriedadeModel][0];
            }
        }
        else {
            $scope.evidenciaSelecionada = null;
        }
    };

    $scope.selecionarTipoEvidencia = function (tipo) {
        $scope.tipoEvidenciaSelecionado = +tipo;

        var nomePropriedade = $scope.tipoEvidenciaSelecionado === TipoEvidenciaEnum.SUCESSO ? 'Evidencias' : 'Erros';
        $scope.evidenciaSelecionada = $scope.testeSelecionado[nomePropriedade][0];

    };

    $scope.upload = function (file) {
        if (!file) return;


        fileUploadService.upload(file, true)
            .then(function (response) {
                $scope.evidenciaSelecionada.GuidImagem = response.data.guid;
            },
            function (response) {
                $scope.notify('Erro ao fazer upload da imagem', 'error');
            });
    };

    $scope.exibirBotaoVoltarEvidencia = function () {
        var index = $scope.obterIndexEvidenciaSelecionada();
        return index > 0;
    };

    $scope.exibirBotaoAvancarEvidencia = function () {
        var index = $scope.obterIndexEvidenciaSelecionada();
        var nomePropriedade = $scope.tipoEvidenciaSelecionado === TipoEvidenciaEnum.SUCESSO ? 'Evidencias' : 'Erros';
        return index < ($scope.testeSelecionado[nomePropriedade].length - 1);
    };

    $scope.voltarEvidencia = function () {
        $scope.tipoEvidenciaSelecionado === TipoEvidenciaEnum.SUCESSO ?
            voltarEvidencia('Evidencias') :
            voltarEvidencia('Erros');
    };

    var voltarEvidencia = function (nomePropriedadeModel) {
        var index = $scope.obterIndexEvidenciaSelecionada() - 1;
        if (index >= 0) {
            $scope.evidenciaSelecionada = $scope.testeSelecionado[nomePropriedadeModel][index];
        }
    };

    $scope.avancarEvidencia = function () {
        $scope.tipoEvidenciaSelecionado === TipoEvidenciaEnum.SUCESSO ?
            avancarEvidencia('Evidencias') :
            avancarEvidencia('Erros');
    };

    var avancarEvidencia = function (nomePropriedadeModel) {
        var index = $scope.obterIndexEvidenciaSelecionada() + 1;
        if (index < $scope.testeSelecionado[nomePropriedadeModel].length) {
            $scope.evidenciaSelecionada = $scope.testeSelecionado[nomePropriedadeModel][index];
        }
    };

    $scope.obterIndexEvidenciaSelecionada = function () {
        if ($scope.tipoEvidenciaSelecionado === TipoEvidenciaEnum.SUCESSO) {
            return $scope.testeSelecionado.Evidencias.indexOf($scope.evidenciaSelecionada);
        }
        else {
            return $scope.testeSelecionado.Erros.indexOf($scope.evidenciaSelecionada);
        }
    };

    $scope.tituloModalEvidencias = function () {
        var titulo = $scope.tipoEvidenciaSelecionado === TipoEvidenciaEnum.SUCESSO ? "Evidência" : "Erro";
        return titulo + ' #' + ($scope.obterIndexEvidenciaSelecionada() + 1);
    };
    // #endregion

    $scope.init();
}]);