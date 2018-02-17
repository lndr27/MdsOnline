app.controller('RTUController', ['$controller', '$scope', function ($controller, $scope) {

    angular.extend(this, $controller('BaseController', { $scope: $scope }));

    $scope.testes = [];

    $scope.init = function () {
        bindSortable();
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

    $scope.init();
}]);