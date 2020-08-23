
var layer = null;
var edit = null;
var pro = { Features1: [{}], Quality1: [{}], props: [{}], ImgUrl1:[]};
layui.use(['layer', "form", 'upload'], function () {
    layer = layui.layer;
    upload = layui.upload;
    upload.render({
        elem: '#test2'
        , url: '/File/ImgUpload?Path=Product' //改成您自己的上传接口
        , multiple: true
        , before: function (obj) {
            loadding();
            obj.preview(function (index, file, result) {
                
            });
        }
        , done: function (res) {
            loadding(false);
            //$('#demo2').append('<label data-url="' + res.data.src + '"><img src="/Upload/Product/' + res.data.src + '" class="layui-upload-img"><span onclick="$(this).parent().remove();"></span></label>');
            pro.ImgUrl1.push(res.data.src);
            //上传完毕
        }, error: function () {
            loadding(false);
        }
    });
    upload.render({
        elem: '#test3'
        , url: '/File/FileUpload?Path=PDF'
        , accept: 'file'
        , exts: 'PDF'
        , before: function (obj) {
            obj.preview(function (index, file, result) {
                pro.PDFSoureName=file.name
                $(".pdf").text(file.name);
            });
        }
        , done: function (res) {
            console.log(res);
            if (res.code == 0) {
                layer.msg('Uploaded successfully!');
                pro.PDF = res.data.src;
            } else {
                layer.msg(res.msg);
            }
        }
    });
    del = function (id) {
        layer.confirm("Are you sure to delete it?", function () {
            $.post("/Product/ProductDel", { Id: id }, function (data) {
                var res = JSON.parse(data);
                if (res.success) {
                    layer.alert('Delete successfully !', function () {
                        location.href = "/Product/Index";
                    })
                }
                else {
                    layer.alert('Delete failed:' + res.msg)
                }
            });
        });
    }
})
new Vue({
    el: "#pro",
    data: {
        IsEdit: false,
        product: pro, 
        list: [
            { id: 1, name: "Abby", sport: "basket" },
            { id: 2, name: "Brooke", sport: "foot" },
            { id: 3, name: "Courtenay", sport: "volley" },
            { id: 4, name: "David", sport: "rugby" }
        ],
        dragging: false
    },
    methods: {
        open(pro1, props) {
            if (pro1) {
                pro.Id = pro1.Id;
                pro.Name = pro1.Name;
                pro.Classify = pro1.Classify;
                pro.PDF = pro1.PDF;
                pro.PDFSoureName = pro1.PDFSoureName;
                pro.Describe = pro1.Describe;
                pro.ProductID = pro1.ProductID;
                pro.ProductCode = pro1.ProductCode;
                pro.ImgUrl1 = (pro1.ImgUrl || "").split("|");
                pro.Features1 = pro1.Features ? pro1.Features.split("|").map(it => { return { value: it }} ) : [{}];
                pro.Quality1 = pro1.Quality ? pro1.Quality.split("|").map(it => { return { value: it } }) : [{}];
                pro.props = props.length == 0 ? [{}] : props;
            } else {
                pro.Id = "";
                pro.Name = "";
                pro.Classify = "";
                pro.PDF = "";
                pro.PDFSoureName = "";
                pro.Describe = "";
                pro.ProductID = "";
                pro.ProductCode = "";
                pro.ImgUrl1 = [];
                pro.Features1 = [{}];
                pro.Quality1 = [{}];
                pro.props = [{}];
            }
            this.IsEdit = !this.IsEdit;
        },
        save() {
            this.product.ImgUrl = this.product.ImgUrl1.join("|");
            this.product.Features = this.product.Features1.map(it => it.value).join("|");
            this.product.Quality = this.product.Quality1.map(it => it.value).join("|");
            $.post("/Product/ProductAdd", { model: this.product, pros: this.product.props }, function (data) {
                loadding(false);
                var res = JSON.parse(data);
                if (res.success) {
                    layer.alert('Save successfully!', function () {
                        location.href = "/Product/Index";
                    })
                }
                else {
                    layer.alert('Save failed:' + res.msg)
                }
            });
        },
        featuresAdd(name) {
            this.product[name].push({});
        },
        featuresDel(name,index) {
            this.product[name].splice(index,1);
        }
    },
    created() {
               
    }
})
var el = document.getElementById('sortTable');
new Sortable(el, {
    animation: 150,
    ghostClass: 'blue-background-class',
    onSort: function (evt) {
        if (evt.newIndex != evt.oldIndex) {
            if (confirm("是否确定保存当前顺序？")) {
                var ids = [];
                $("[data-id]").each(function () {
                    ids.push($(this).data("id"));
                })
                //console.log(ids);
                $.post("/Product/ProductSort", { ids: ids }, function (res) {
                    console.log(res);
                    res = JSON.parse(res);
                    if (res.success) {
                        alert("排序成功！");
                    }
                })
            }
        }        
    }
}); 