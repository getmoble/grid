function ProfileViewModel() {
    var self = this;

    self = ViewModelBase(self);

    self.userId = ko.observable();

    self.addSkill = function () {
        bootbox.confirm("Are you sure you want to add this skill", function (result) {
            if (result) {
                $.ajax({
                    type: "POST",
                    data: { UserId: self.userId(), SkillId: $("#SkillId").val() },
                    url: '/Profile/AddSkill/',
                    success: function () {
                        location.reload();
                    },
                });
            }
        });
    };

    self.addCertification = function () {
        bootbox.confirm("Are you sure you want to add this certification", function (result) {
            if (result) {
                $.ajax({
                    type: "POST",
                    data: { UserId: self.userId(), CertificationId: $("#CertificationId").val() },
                    url: '/Profile/AddCertification/',
                    success: function () {
                        location.reload();
                    },
                });
            }
        });
    };

    self.init = function (id) {
        self.userId(id);
    };
}