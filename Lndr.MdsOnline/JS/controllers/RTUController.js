app.controller('RTUController', ['$controller', '$scope', 'MDSOnlineService', function ($controller, $scope, service) {

    "use strict";

    angular.extend(this, $controller('BaseController', { $scope: $scope }));

    $scope.testes = [];

    $scope.edicaoHabilitada = false;

    $scope.init = function () {
        bindSortable();

        $(document).off("keydown", onKeyupListener).on("keydown", onKeyupListener);

        service.obterRTU($scope.chamado)
            .then(function (response) {
                $scope.testes = response.data.Testes;
            },
            function () {
            });
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

    $scope.salvar = function () {

        service.salvarRTU({ Chamado: $scope.chamado, Testes: $scope.testes })
            .then(function (response) {
                alertify.success("Documento salvo com sucesso!");
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
        if (sortble.data('uiSortable')) {
            sortble.sortable("destroy");
        }
        sortble.sortable({ handle: '.sortable-handle' });
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

    $scope.habilitarDesabilitarEdicao = function () { $scope.edicaoHabilitada = !$scope.edicaoHabilitada; };

    $scope.init();
}]);