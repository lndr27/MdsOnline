app.config(['$httpProvider', function ($httpProvider) {

    $httpProvider.interceptors.push(function ($q) {
        return {
            'response': function (response) {
                parseDatas(response.data);
                return response;
            }
        };
    });

    function parseDatas(obj) {
        for (var prop in obj) {
            if (!obj.hasOwnProperty(prop)) continue;

            var regex = /date\(/gi;

            if (typeof obj[prop] === 'object') {
                parseDatas(obj[prop]);
            }
            else if (typeof obj[prop] === 'string' && regex.test(obj[prop]) === true) {
                var num = +(/\d+/g.exec(obj[prop])[0]);
                obj[prop] = new Date(num);
            }
        }
    }
}]);