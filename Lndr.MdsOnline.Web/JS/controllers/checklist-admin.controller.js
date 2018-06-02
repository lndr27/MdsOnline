"use strict";

app.controller('AdminCheckListController', ['$controller', '$scope', 'MDSOnlineService', function ($controller, $scope, service) {

    angular.extend(this, $controller('BaseController', { $scope: $scope }));

    $scope.checklistId = 0;

    $scope.checklist = {};

    $scope.edicaoHabilitada = false;

    $scope.testesRemovidos = [];

    $scope.init = function () {

        $scope.checklistId = +$('#checklistId').val();

        window.debug = $scope;

        carregarChecklist();
    };

    var carregarChecklist = function () {
        service.obterCheckListAdmin($scope.checklistId)
            .then(function (response) {
                $scope.checklist = response.data;
            },
            $scope.erroInsesperado);
    };

    $scope.habilitarDesabilitarEdicao = function () {
        $scope.edicaoHabilitada = !$scope.edicaoHabilitada;
    };

    $scope.adicionarNovoGrupo = function () {
        $scope.checklist.GruposItens.push({
            Nome: '',
            Descricao: '',
            Itens: []
        });
    };

    $scope.adicionarNovoItem = function (grupo) {
        grupo.Itens.push({
            Nome: '',
            Descricao: ''
        });
    };

    $scope.init();
}]);