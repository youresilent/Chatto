$(function () {

	var hubVar = $.connection.signalHub;

	hubVar.client.showNotification = function (userName, message) {
		document.getElementById('notificationBlock').removeAttribute('hidden');
		document.getElementById('notificationBlock').append(htmlEncode(userName) + ' ' + htmlEncode(message));

		$.fn.playSound();

		setTimeout(function () {
			window.location.reload();
		}, 2000);
	};

	hubVar.client.addMessage = function (sender, recipient, message, dateTime) {

		var dateObject = new Date(dateTime);
		$("#chatbox").val($("#chatbox").val() + '[' + dateObject.toLocaleString() + ']\n' + htmlEncode(sender) + ': ' + htmlEncode(message) + '\n\n');

		$('#chatbox').scrollTop($('#chatbox')[0].scrollHeight);
	};

	$.fn.playSound = function () {
		const audio = new Audio("/Sounds/ping.wav");
		audio.play();
	}

	hubVar.client.onNewUserConnected = function (id, name) {
		AddUser(id, name);
	}

	hubVar.client.onConnected = function (id, userName, allUsers) {
		for (i = 0; i < allUsers.length; i++) {
			AddUser(allUsers[i].ConnectionId, allUsers[i].User);
		}
	}

	hubVar.client.onUserDisconnected = function (id, userName) {
		$('#' + id).remove();
	}

	$.connection.hub.start().done(function () {
		console.log('connected! ' + hubVar.connection.id);

		//hubVar.server.connect(document.getElementById('userName').getAttribute('value'));
		hubVar.server.connect($('.userName').val());

		$("#Send").click(function () {

			var senderName = $('.userName').val();
			var recipientName = $('.friendUserName').val();
			var sendMessageText = $('.messageText').val();

			if (sendMessageText.length == 0)
				return;

			$.ajax({
				type: 'POST',
				url: '/Chat/ChatAction',
				data: {
					messageText: sendMessageText,
					currentUserName: senderName,
					friendUserName: recipientName
				},
				success: function (result) {
					$('.messageText').val("");
				}
			});

		});
	})

	function htmlEncode(value) {
		var encodedValue = $('<div />').text(value).html();

		return encodedValue;
	}

	function AddUser(id, name) {
		var userId = $('#hdId').val();

		if (userId != id) {
			$("#chatusers").append('<p id="' + id + '"><b>' + name + '</b></p>');
		}
	}
})