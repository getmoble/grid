﻿(function ($) {
    $.fn.feedback = function () {
        self = $(this);
        $(this).find('.dropdown-menu-form').on('click', function (e) { e.stopPropagation() })

        $(this).find('.screenshot').on('click', function () {
            $this = $(this);
            $this.find('i').removeClass('fa-camera fa-check').addClass('fa-refresh fa-spin');
            html2canvas($(document.body), {
                onrendered: function (canvas) {
                    $('.screen-uri').val(canvas.toDataURL("image/png"));
                    $this.find('i').removeClass('fa-refresh fa-spin').addClass('fa-check');
                }
            });
        });

        $(this).find('.do-close').on('click', function () {
            self.find('.dropdown-toggle').dropdown('toggle');
            self.find('.reported, .failed').hide();
            self.find('.report').show();
        });

        $(this).on('submit', 'form', function () {
            $.ajax({
                type: "POST",
                data: {
                    Comment: $("#comment").val(),
                    Screenshot: $("#screenshot").val()
                },
                url: '/Settings/UserFeedbacks/Create',
                success: function (result) {
                    self.find('.report').hide();
                    self.find('.reported').show();
                },
                error: function () {
                    self.find('.report').hide();
                    self.find('.failed').show();
                }
            });

            return false;
        });
    };
}(jQuery));

$('.feedback').feedback();