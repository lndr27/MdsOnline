﻿/* mousetrap v1.6.1 craig.is/killing/mice */
(function (r, v, f) {
    function w(a, b, g) { a.addEventListener ? a.addEventListener(b, g, !1) : a.attachEvent("on" + b, g) } function A(a) { if ("keypress" == a.type) { var b = String.fromCharCode(a.which); a.shiftKey || (b = b.toLowerCase()); return b } return p[a.which] ? p[a.which] : t[a.which] ? t[a.which] : String.fromCharCode(a.which).toLowerCase() } function F(a) { var b = []; a.shiftKey && b.push("shift"); a.altKey && b.push("alt"); a.ctrlKey && b.push("ctrl"); a.metaKey && b.push("meta"); return b } function x(a) {
        return "shift" == a || "ctrl" == a || "alt" == a ||
            "meta" == a
    } function B(a, b) { var g, c, d, f = []; g = a; "+" === g ? g = ["+"] : (g = g.replace(/\+{2}/g, "+plus"), g = g.split("+")); for (d = 0; d < g.length; ++d)c = g[d], C[c] && (c = C[c]), b && "keypress" != b && D[c] && (c = D[c], f.push("shift")), x(c) && f.push(c); g = c; d = b; if (!d) { if (!n) { n = {}; for (var q in p) 95 < q && 112 > q || p.hasOwnProperty(q) && (n[p[q]] = q) } d = n[g] ? "keydown" : "keypress" } "keypress" == d && f.length && (d = "keydown"); return { key: c, modifiers: f, action: d } } function E(a, b) { return null === a || a === v ? !1 : a === b ? !0 : E(a.parentNode, b) } function c(a) {
        function b(a) {
            a =
                a || {}; var b = !1, l; for (l in n) a[l] ? b = !0 : n[l] = 0; b || (y = !1)
        } function g(a, b, u, e, c, g) { var l, m, k = [], f = u.type; if (!h._callbacks[a]) return []; "keyup" == f && x(a) && (b = [a]); for (l = 0; l < h._callbacks[a].length; ++l)if (m = h._callbacks[a][l], (e || !m.seq || n[m.seq] == m.level) && f == m.action) { var d; (d = "keypress" == f && !u.metaKey && !u.ctrlKey) || (d = m.modifiers, d = b.sort().join(",") === d.sort().join(",")); d && (d = e && m.seq == e && m.level == g, (!e && m.combo == c || d) && h._callbacks[a].splice(l, 1), k.push(m)) } return k } function f(a, b, c, e) {
            h.stopCallback(b,
                b.target || b.srcElement, c, e) || !1 !== a(b, c) || (b.preventDefault ? b.preventDefault() : b.returnValue = !1, b.stopPropagation ? b.stopPropagation() : b.cancelBubble = !0)
        } function d(a) { "number" !== typeof a.which && (a.which = a.keyCode); var b = A(a); b && ("keyup" == a.type && z === b ? z = !1 : h.handleKey(b, F(a), a)) } function p(a, c, u, e) {
            function l(c) { return function () { y = c; ++n[a]; clearTimeout(r); r = setTimeout(b, 1E3) } } function g(c) { f(u, c, a); "keyup" !== e && (z = A(c)); setTimeout(b, 10) } for (var d = n[a] = 0; d < c.length; ++d) {
                var m = d + 1 === c.length ? g : l(e ||
                    B(c[d + 1]).action); q(c[d], m, e, a, d)
            }
        } function q(a, b, c, e, d) { h._directMap[a + ":" + c] = b; a = a.replace(/\s+/g, " "); var f = a.split(" "); 1 < f.length ? p(a, f, b, c) : (c = B(a, c), h._callbacks[c.key] = h._callbacks[c.key] || [], g(c.key, c.modifiers, { type: c.action }, e, a, d), h._callbacks[c.key][e ? "unshift" : "push"]({ callback: b, modifiers: c.modifiers, action: c.action, seq: e, level: d, combo: a })) } var h = this; a = a || v; if (!(h instanceof c)) return new c(a); h.target = a; h._callbacks = {}; h._directMap = {}; var n = {}, r, z = !1, t = !1, y = !1; h._handleKey = function (a,
            c, d) { var e = g(a, c, d), k; c = {}; var h = 0, l = !1; for (k = 0; k < e.length; ++k)e[k].seq && (h = Math.max(h, e[k].level)); for (k = 0; k < e.length; ++k)e[k].seq ? e[k].level == h && (l = !0, c[e[k].seq] = 1, f(e[k].callback, d, e[k].combo, e[k].seq)) : l || f(e[k].callback, d, e[k].combo); e = "keypress" == d.type && t; d.type != y || x(a) || e || b(c); t = l && "keydown" == d.type }; h._bindMultiple = function (a, b, c) { for (var d = 0; d < a.length; ++d)q(a[d], b, c) }; w(a, "keypress", d); w(a, "keydown", d); w(a, "keyup", d)
    } if (r) {
        var p = {
            8: "backspace", 9: "tab", 13: "enter", 16: "shift", 17: "ctrl",
            18: "alt", 20: "capslock", 27: "esc", 32: "space", 33: "pageup", 34: "pagedown", 35: "end", 36: "home", 37: "left", 38: "up", 39: "right", 40: "down", 45: "ins", 46: "del", 91: "meta", 93: "meta", 224: "meta"
        }, t = { 106: "*", 107: "+", 109: "-", 110: ".", 111: "/", 186: ";", 187: "=", 188: ",", 189: "-", 190: ".", 191: "/", 192: "`", 219: "[", 220: "\\", 221: "]", 222: "'" }, D = { "~": "`", "!": "1", "@": "2", "#": "3", $: "4", "%": "5", "^": "6", "&": "7", "*": "8", "(": "9", ")": "0", _: "-", "+": "=", ":": ";", '"': "'", "<": ",", ">": ".", "?": "/", "|": "\\" }, C = {
            option: "alt", command: "meta", "return": "enter",
            escape: "esc", plus: "+", mod: /Mac|iPod|iPhone|iPad/.test(navigator.platform) ? "meta" : "ctrl"
        }, n; for (f = 1; 20 > f; ++f)p[111 + f] = "f" + f; for (f = 0; 9 >= f; ++f)p[f + 96] = f.toString(); c.prototype.bind = function (a, b, c) { a = a instanceof Array ? a : [a]; this._bindMultiple.call(this, a, b, c); return this }; c.prototype.unbind = function (a, b) { return this.bind.call(this, a, function () { }, b) }; c.prototype.trigger = function (a, b) { if (this._directMap[a + ":" + b]) this._directMap[a + ":" + b]({}, a); return this }; c.prototype.reset = function () {
            this._callbacks = {};
            this._directMap = {}; return this
        }; c.prototype.stopCallback = function (a, b) { return -1 < (" " + b.className + " ").indexOf(" mousetrap ") || E(b, this.target) ? !1 : "INPUT" == b.tagName || "SELECT" == b.tagName || "TEXTAREA" == b.tagName || b.isContentEditable }; c.prototype.handleKey = function () { return this._handleKey.apply(this, arguments) }; c.addKeycodes = function (a) { for (var b in a) a.hasOwnProperty(b) && (p[b] = a[b]); n = null }; c.init = function () {
            var a = c(v), b; for (b in a) "_" !== b.charAt(0) && (c[b] = function (b) {
                return function () {
                    return a[b].apply(a,
                        arguments)
                }
            }(b))
        }; c.init(); r.Mousetrap = c; "undefined" !== typeof module && module.exports && (module.exports = c); "function" === typeof define && define.amd && define(function () { return c })
    }
})("undefined" !== typeof window ? window : null, "undefined" !== typeof window ? document : null);