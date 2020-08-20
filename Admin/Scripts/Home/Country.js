var edit = null;
var del = null;
layui.use(['layer', "form", 'upload'], function () {
    var $ = layui.jquery;
    layer = layui.layer;
    var form = layui.form;
    upload = layui.upload;
    edit = function (obj) {
        //清空操作
        if (!obj) {
            form.val('example1', { Country1: "", Url: "" });
        } else {
            form.val('example1', obj);
        }
        layer.open({
            type: 1
            , title: 'Country'
            , area: ['665px', '465px']
            , content: $('#proEdit')
            , btn: ['Save', 'Cancel'] //只是为了演示
            , yes: function () {
                var data = form.val('example1');
                $.post("/Setting/CountrySave", data, function (data) {
                    var res = JSON.parse(data);
                    if (res.success) {
                        layer.alert('Save successfully!', function () {
                            location.href = "/Setting/CountrySelect";
                        })
                    }
                    else {
                        layer.alert('Save failed:' + res.msg)
                    }
                });
            }
            , btn2: function () {

            }
        });
    }
    del = function (id) {
        layer.confirm("Are you sure to delete it?", function () {
            $.post("/Setting/CountryDel", { Id: id }, function (data) {
                var res = JSON.parse(data);
                if (res.success) {
                    layer.alert('Delete successfully!', function () {
                        location.href = "/Setting/CountrySelect";
                    })
                }
                else {
                    layer.alert('Delete failed:' + res.msg)
                }
            });
        });
    }
    $(".btn-add").click(function () {
        edit();
    })
})