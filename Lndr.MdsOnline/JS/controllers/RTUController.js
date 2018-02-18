app.controller('RTUController', ['$controller', '$scope', 'MDSOnlineService', function ($controller, $scope, service) {

    "use strict";

    angular.extend(this, $controller('BaseController', { $scope: $scope }));

    $scope.testes = [];

    $scope.edicaoHabilitada = false;

    $scope.init = function () {
        $(document).off("keydown", onKeyupListener).on("keydown", onKeyupListener);
        bindSortable();
        carregarTestes();
    };

    var onKeyupListener = function (evt) {
        if (event.ctrlKey || event.metaKey) {
            switch (String.fromCharCode(event.which).toLowerCase()) {
                case 's':
                    event.preventDefault();
                    $scope.salvar();
                    break;
            }
        }
    };

    var carregarTestes = function () {
        service.obterRTU($scope.chamado)
            .then(function (response) {
                $scope.testes = response.data.Testes;
            },
            function () {
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
            Verificacao: '' + StatusTesteUnitarioEnum.NAO_TESTADO
        });
        bindSortable();
    };

    $scope.removerTesteUnitario = function (teste) {
        $scope.testes.splice($scope.testes.indexOf(teste), 1);
        bindSortable();
    };

    var bindSortable = function () {
        if (!$scope.edicaoHabilitada) {
            return;
        }

        var sortble = $('.sortable');
        sortble.sortable({
            handle: '.sortable-handle',
            change: atualizaOrdemTestes
        });
    };

    var atualizaOrdemTestes = function (evt, ui) {
        _.forEach($scope.testes, function (teste, i) {
            teste.Ordem = i;
        });
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
        if ($scope.edicaoHabilitada) {
            bindSortable();
            $('.sortabe').sortable('enable');
        }
        else {
            $('.sortabe').sortable('disable');
        }
    };

    $scope.init();
}]);