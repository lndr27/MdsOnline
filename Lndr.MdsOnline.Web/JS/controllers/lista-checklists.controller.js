"use strict";

app.controller('ListaCheckListsController', ['$controller', '$scope', 'MDSOnlineService', function ($controller, $scope, service) {

    angular.extend(this, $controller('BaseController', { $scope: $scope }));

    $scope.CheckLists = 0;

    $scope.Pagina = 1;

    $scope.TamanhoPagina = 10;

    $scope.TotalPaginas = 0;

    $scope.PossuiProximaPagina = false;

    $scope.PossuiPaginaAnterior = false;

    $scope.ProximaPagina = 0;

    $scope.PaginaAnterior = 0;

    $scope.init = function () {

        carregarChecklists();
    };

    var carregarChecklists = function () {
        service.obterListaCheckLists($scope.Pagina, $scope.TamanhoPagina)
            .then(function (response) {
                $.extend($scope, response.data);
            },
            $scope.erroInsesperado);
    };

    $scope.init();
}]);