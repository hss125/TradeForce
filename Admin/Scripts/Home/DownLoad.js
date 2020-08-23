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
            form.val('example1', { Name: "", Describe: "", Url: "" });
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
                $.post("/Home/DownLoadSave", data, function (data) {
                    var res = JSON.parse(data);
                    if (res.success) {
                        layer.alert('Save successfully!', function () {
                            location.href = "/Home/DownLoad";
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
            $.post("/Home/DownLoadDel", { Id: id }, function (data) {
                var res = JSON.parse(data);
                if (res.success) {
                    layer.alert('Delete successfully!', function () {
                        location.href = "/Home/DownLoad";
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
    //指定允许上传的文件类型
    upload.render({
        elem: '#test3'
        , url: '/File/FileUpload?Path=DownLoad' //改成您自己的上传接口
        , accept: 'file' //普通文件
        , before: function (obj) {
            obj.preview(function (index, file, result) {
                $("#fileName").text(file.name);
            });
        }
        , done: function (res) {
            if (res.code == 0) {
                layer.msg('Uploaded successfully!');
                $("#url").val(res.data.src);
            } else {
                layer.msg(res.msg);
            }
        }
    });
})