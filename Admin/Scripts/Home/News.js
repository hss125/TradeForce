
var E = window.wangEditor
var editor = new E('#div3')
editor.customConfig.menus = ['head', 'bold', 'fontSize', 'italic', 'underline', 'strikeThrough',
    'foreColor', 'backColor', 'link', 'list', 'justify', 'quote', 'image', 'table',
    'video', 'code', 'undo',]
editor.customConfig.uploadImgServer = '/File/ImgUpload2'
editor.create()
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
            form.val('example1', { Title: "", LangTitle: "" });
            $("#demo1").attr("src", "");
            editor.txt.clear();
        } else {
            form.val('example1', obj);
            $("#demo1").attr("src", "/Upload/News/" + obj.Img);
            editor.txt.html(obj.Content)
        }
        layer.open({
            type: 1
            , title: 'News'
            , area: ['880px', '550px']
            , content: $('#proEdit')
            , btn: ['Save', 'Cancel'] //只是为了演示
            , yes: function () {
                var data = form.val('example1');
                data.Content = editor.txt.html();
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