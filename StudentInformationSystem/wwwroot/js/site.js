$('.reply-ticket-btn').click(function () {
    console.log("Work!");
    var ticketId = $(this).data('ticket-id');
    var userNameWhoReplies = $(this).data('user-name');
    $('#replyTicketId').val(ticketId);
    $('#userNameWhoReplies').val(userNameWhoReplies);
    $('#replyTicketModal').modal('show');
});

$('#replyTicketForm').submit(function (e) {
    $('#replyTicketModal').modal('hide');
});