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
            form.val('example1', { Title: "", Content: "" });
            $("#demo1").attr("src", "");
        } else {
            form.val('example1', obj);
            $("#demo1").attr("src", "/Upload/News/" + obj.Img);
        }
        layer.open({
            type: 1
            , title: 'News'
            , area: ['665px', '465px']
            , content: $('#proEdit')
            , btn: ['Save', 'Cancel'] //只是为了演示
            , yes: function () {
                var data = form.val('example1');
                $.post("/Home/NewsSave", data, function (data) {
                    var res = JSON.parse(data);
                    if (res.success) {
                        layer.alert('Save successfully!', function () {
                            location.href = "/Home/News";
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
            $.post("/Home/NewsDel", { Id: id }, function (data) {
                var res = JSON.parse(data);
                if (res.success) {
                    layer.alert('Delete successfully!', function () {
                        location.href = "/Home/News";
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
    var index = null;
    //普通图片上传
    var uploadInst = upload.render({
        elem: '#test1'
        , url: '/File/ImgUpload?Path=News'
        , before: function (obj) {
            obj.preview(function (index, file, result) {
                $('#demo1').attr('src', result); 
            });
            index = layer.load(1, {
                shade: [0.2, '#fff'] 
            });
        }
        , done: function (res) {
            console.log(res);
            layer.close(index);
            if (res.code > 0) {
                return layer.msg('Uploaded failed!');
            }
            $("#ImgUrl").val(res.data.src);
        }
        , error: function () {
            layer.close(index);
            var demoText = $('#demoText');
            demoText.html('<span style="color: #FF5722;">Uploaded failed</span> <a class="layui-btn layui-btn-xs demo-reload">Retry</a>');
            demoText.find('.demo-reload').on('click', function () {
                uploadInst.upload();
            });
        }
    });
})