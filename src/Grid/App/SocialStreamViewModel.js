function Post(post) {
    var self = ViewModelBase(this);
    self.Id = post.Id;
    self.Title = post.Title;
    self.Content = post.Content;
    self.CreatedByUserName = post.CreatedByUserName;
    self.CreatedByUserImage = "https://gridstores.blob.core.windows.net/profile/4cd640d80894400ea650b50ca9eb278b.png";
    self.CreatedOn = self.parseDate(post.CreatedOn).toLocaleDateString() + " " + self.parseDate(post.CreatedOn).toLocaleTimeString();
}

function SocialStreamViewModel() {
    var self = this;

    self = ViewModelBase(self);

    self.id = ko.observable();
    self.posts = ko.observableArray([]);
    self.post = ko.observable();

    self.addNewPost = function () {
        $.ajax({
            type: "POST",
            data: { Title: $("#newNoteTitle").val(), Content: $("#newNote").val() },
            url: '/Home/AddPost/',
            success: function () {
                location.reload();
            },
        });
    };

    self.getAllPosts = function () {
        $.getJSON('/Home/GetAllPosts', function (data) {
            $.each(data, function (k, v) {
                self.posts.push(new Post(v));
            });
        });
    };

    self.init = function () {
        self.getAllPosts();
    };
}