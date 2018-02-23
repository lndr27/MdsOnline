"use strict";
app.controller('RTUController', ['$controller', '$scope', 'MDSOnlineService', function ($controller, $scope, service) {    

    angular.extend(this, $controller('BaseController', { $scope: $scope }));

    $scope.testes = [];

    $scope.edicaoHabilitada = false;

    $scope.sortableOptions = { 'ui-preserve-size': true, handle: '.sortable-handle', cancel: ".not-sortable" };

    $scope.init = function () {        
        carregarTestes();
        initAtalhosTeclado();
    };

    var carregarTestes = function () {
        service.obterRTU($scope.chamado)
            .then(function (response) {
                $scope.testes = response.data.Testes;
            },
            function () {
            });
    };

    var initAtalhosTeclado = function () {
        $(document).on("keydown", function (evt) {

            var isCtrlPressionado = event.ctrlKey || event.metaKey;
            var caracterPressionado = String.fromCharCode(event.which).toUpperCase();

            if (isCtrlPressionado && caracterPressionado === 'S') {
                evt.preventDefault();
                $scope.salvar();
            }
        });
    };

    $scope.salvar = function () {

        if (!$scope.edicaoHabilitada) return;

        $scope.confirmar("Confirmação", "Deseja salvar suas alterações?", function (confirm) {
            if (confirm) {
                service.salvarRTU({ Chamado: $scope.chamado, Testes: $scope.testes })
                    .then(function (response) {
                        alertify.success("Documento salvo com sucesso!");
                    });
            }
        });        
    };

    $scope.adicionarNovoTesteUnitario = function () {
        $scope.testes.push({
            Sequencia: ($scope.testes.length || 0) + 1,
            Verificacao: '' + StatusTesteUnitarioEnum.NAO_TESTADO,
            Ordem: $scope.testes.length + 1
        });
    };

    $scope.removerTesteUnitario = function (teste) {
        $scope.testes.splice($scope.testes.indexOf(teste), 1);
        bindSortable();
    };
   
    $scope.obterClasseCelulaVerificacao = function (verificacao) {
        switch (+verificacao) {
            case StatusTesteUnitarioEnum.OK:
                return "teste-ok";
            case StatusTesteUnitarioEnum.NOK:
                return "teste-nok";
            case StatusTesteUnitarioEnum.NAO_TESTADO:
                return "teste-nao-testado";
        }
    };

    $scope.habilitarDesabilitarEdicao = function () {
        $scope.edicaoHabilitada = !$scope.edicaoHabilitada;
    };

    $scope.init();
}]);