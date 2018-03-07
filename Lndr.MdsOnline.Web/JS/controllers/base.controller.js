app.controller('BaseController', ['$scope', '$timeout', function ($scope, $timeout) {

    $scope.init = function () {
        $scope.chamado = +$('input[name="chamado"').val();
        $scope.enableTooltips();
        setupAlertify();
    };

    var setupAlertify = function () {
        alertify.defaults.transition = 'slide';
        alertify.defaults.theme.ok = 'btn btn-primary';
        alertify.defaults.theme.cancel = 'btn btn-secondary';
        alertify.defaults.theme.input = 'from-control';
        alertify.defaults.glossary.ok = 'OK';
        alertify.defaults.glossary.cancel = 'Cancelar';
        alertify.defaults.glossary.title = '';
        alertify.defaults.notifier.delay = 3;
    };

    $scope.enableTooltips = function () {
        $timeout(function () {
            $('[data-toggle="tooltip"]').tooltip();
            $('[data-toggle="tooltip"]').tooltip('hide');
        }, 0);

    };

    $scope.confirmar = function (titulo, mensagem, cb) {
        alertify.defaults.glossary.title = titulo || 'Confirmação';
        alertify.confirm(mensagem, cb);
    };

    $scope.erroInsesperado = function () {
        $scope.notify('Ocorreu um erro inesperado.', 'error');
    };

    $scope.exibirMensagemCamposComErros = function (camposComErros) {
        _.forEach(camposComErros, function (campo) {
            if (campo.Erros) {
                _.forEach(campo.Erros, function (erro) {
                    $scope.notify(erro, 'error');
                });
            }
        });
    };

    var msgTemplate = '<div class="row"><div class="col-md-2">@icon</div><div class="col-md-10">@msg</div></div>';
    var iconesNotificacao = {
        erro: '<i class="fa fa-exclamation-circle" aria-hidden="true"></i>',
        sucesso: '<i class="fa fa-check" aria-hidden="true"></i>',
        aviso: '<i class="fa fa-exclamation-triangle" aria-hidden="true"></i>',
        info: '<i class="fa fa-comment-o" aria-hidden="true"></i>',
        undo: '<i class="fa fa-undo" aria-hidden="true"></i>',
    };

    $scope.notify = function (msg, nomeIcone, delay, cb) {

        var icone = "";
        var tipoNotificacao = 'message';
        switch ((nomeIcone || '').toLowerCase()) {
            case 'error':
                icone = iconesNotificacao.erro;
                tipoNotificacao = 'error';
                break;
            case 'success':
                icone = iconesNotificacao.sucesso;
                tipoNotificacao = 'success';
                break;
            case 'warning':
                icone = iconesNotificacao.aviso;
                tipoNotificacao = 'warning';
                break;
            case 'message':
                icone = iconesNotificacao.info;
                tipoNotificacao = 'message';
                break;
            case 'undo':
                icone = iconesNotificacao.undo;
                break;
        }
        msg = msgTemplate.replace('@icon', icone).replace('@msg', msg);
        delay = (delay >= 0) ? delay : alertify.defaults.notifier.delay;
        alertify[tipoNotificacao](msg, delay, cb);
    };

    $scope.abrirModal = function (id, delay) {
        var elemento = $(id.indexOf('#') !== - 1 ? id : '#' + id);
        setTimeout(function () {
            elemento.modal('show');
        }, delay || 0);
    };

    $scope.fecharModal = function (id, delay) {
        var elemento = $(id.indexOf('#') !== - 1 ? id : '#' + id);
        setTimeout(function () {
            elemento.modal('hide');
        }, delay || 0);
    };

    $scope.linkClick = function (url) {
        var link = $("<a></a>");
        link.attr('href', url);
        link.css('display', 'none');
        link.attr('target', '_blank');
        $('body').append(link);
        link[0].click();
        link.remove();
    };    

    $scope.init();

}]);