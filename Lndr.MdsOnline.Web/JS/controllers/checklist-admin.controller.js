"use strict";

app.controller('AdminCheckListController', ['$controller', '$scope', 'MDSOnlineService', function ($controller, $scope, service) {

    angular.extend(this, $controller('BaseController', { $scope: $scope }));

    $scope.checklistId = 0;

    $scope.model = {};

    $scope.edicaoHabilitada = false;

    $scope.testesRemovidos = [];

    $scope.sortableOptions = { 'ui-preserve-size': true, handle: '.sortable-handle', cancel: ".not-sortable" };

    $scope.init = function () {

        $scope.checklistId = +$('#checklistId').val();

        carregarChecklist();

        initAtalhosTeclado();
    };

    var carregarChecklist = function () {
        service.obterCheckListAdmin($scope.checklistId)
            .then(function (response) {
                $scope.model = response.data;

                if ($scope.model.GruposItens.length === 0) {
                    $scope.adicionarNovoGrupo();
                    $scope.adicionarNovoItem($scope.model.GruposItens[0]);
                }
            },
            $scope.erroInsesperado);
    };

    var initAtalhosTeclado = function () {
        Mousetrap.bind(['command+s', 'ctrl+s'], function (e) {
            $scope.salvarChecklist();
            return false;
        });
    };

    $scope.habilitarDesabilitarEdicao = function () {
        $scope.edicaoHabilitada = !$scope.edicaoHabilitada;
    };

    $scope.adicionarNovoGrupo = function () {
        $scope.model.GruposItens.push({
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

    $scope.removerItem = function (grupo, item) {
        grupo.Itens = $scope.arrayRemove(grupo.Itens, item);
    };

    $scope.removerGrupo = function (grupo) {
        $scope.model.GruposItens = $scope.arrayRemove($scope.model.GruposItens, grupo);
    };

    $scope.salvarChecklist = function () {
        if (!$scope.edicaoHabilitada) return;

        $scope.confirmar("Confirmação", "Deseja salvar suas alterações?", function (confirm) {
            if (confirm) {
                service.salvarChecklist($scope.model)
                    .then(function (response) {
                        alertify.success("Documento salvo com sucesso!");
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