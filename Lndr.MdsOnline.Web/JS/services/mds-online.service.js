/**
* Servico utiizado para comunicacao do front-end (angular) com backend .NET MVC
*/
"use strict";
app.service("MDSOnlineService", ['$http', function ($http) {

    return {

        /**
        * Obtem RTU de um chamado
        * @param {String} chamado
        * @returns {Promise}
        */
        obterRTU: function (chamado) {
            return $http.get('/RTU/ObterRTU', { params: { chamado: chamado } });
        },

        /**
        * Salva testes do RTU
        * @param {any} rtu - Objeto com lista e testes e ID do chamado ex: { Chamado: 123, Testes: [] }
        * @returns {Promise}
        */
        salvarRTU: function (rtu) {
            return $http.post('/RTU/SalvarRTU', { model: rtu });
        },

        /**
        * Obtem RTF de um chamado
        * @param {String} chamado
        * @returns {Promise}
        */
        obterRTF: function (chamado) {
            return $http.get('/RTF/ObterRTF', { params: { chamado: chamado } });
        },

        /**
        * Salva testes do RTF
        * @param {any} rtf - Objeto com lista e testes e ID do chamado ex: { Chamado: 123, Testes: [] }
        * @returns {Promise}
        */
        salvarRTF: function (rtf) {
            return $http.post('/RTF/SalvarRTF', { model: rtf });
        },

        /**
        * Obtem checklist por ID para edicao dos seus itens e grupos
        * @param {Number} checklistId
        * @returns {Promise}
        */
        obterCheckListAdmin: function (checklistId) {
            return $http.post('/AdminCheckList/ObterCheckList', { checklistId: checklistId });
        },

        /**
         *
         */
        novoCheckList: function () {
            return $http.post('/AdminCheckList/NovoCheckList');
        },

        /**
        * Salvar checklist
        * @param {Object} checklist
        * @returns {Promise}
        */
        salvarChecklist: function (checklist) {
            return $http.post('/AdminCheckList/SalvarCheckList', { model: checklist });
        },

        /**
         * Obtem lista de todos os checklists cadastrados
         * @param {Number} pagina
         * @param {Number} tamanhoPagina
         * @returns {Promise}
         */
        obterListaCheckLists: function (pagina, tamanhoPagina) {
            return $http.post('/AdminCheckList/ObterListaCheckLists', { pagina: pagina, tamanhoPagina: tamanhoPagina});
        }

    };
}]);