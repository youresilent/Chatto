$(function () {

	var hubVar = $.connection.signalHub;

	hubVar.client.showFriendNotification = function (userName, message) {
		document.getElementById('notificationBlock').removeAttribute('hidden');
		document.getElementById('notificationBlock').append(htmlEncode(userName) + ' ' + htmlEncode(message));

		$.fn.playNotificationSound();

		setTimeout(function () {
			window.location.reload();
		}, 2000);
	};

	hubVar.client.showMessageNotification = function (userName, message) {

		var title = document.getElementsByTagName("title")[0].innerHTML;

		if (title == "Chatting - Chatto") {
			var friendName = $('.friendUserName').val();

			if (userName != friendName) {
				notificationBlock.removeAttribute('hidden');
				document.getElementById('messageNotificationBlock').append(htmlEncode(userName) + ' ' + htmlEncode(message));
			}
		}

		if (title != "Chatting - Chatto") {

			var notificationBlock = document.getElementById('messageNotificationBlock');

			if (notificationBlock.hasAttribute('hidden')) {
				notificationBlock.removeAttribute('hidden');
				document.getElementById('messageNotificationBlock').append(htmlEncode(userName) + ' ' + htmlEncode(message));
			}
			else {
				document.getElementById('messageNotificationBlock').append('\n' + htmlEncode(userName) + ' ' + htmlEncode(message));
			}

		}

		$.fn.playMessageSound();
	}

	hubVar.client.addMessage = function (sender, recipient, message, dateTime, friendName) {

		var title = document.getElementsByTagName("title")[0].innerHTML;
		var friendpageName = $('.friendUserName').val();
		var userName = $('.userName').val()

		if (title == "Chatting - Chatto") {

			if (friendpageName == friendName || userName == friendName) {
				var dateObject = new Date(dateTime);
				$("#chatbox").val($("#chatbox").val() + '[' + dateObject.toLocaleString() + ']\n' + htmlEncode(sender) + ': ' + htmlEncode(message) + '\n\n');

				$('#chatbox').scrollTop($('#chatbox')[0].scrollHeight);
			}
		}
	};

	$.fn.playNotificationSound = function () {
		const audio = new Audio("/Sounds/ping.wav");
		audio.play();
	}

	$.fn.playMessageSound = function () {
		const audio = new Audio("/Sounds/message.wav");
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
		console.log($('.friendUserName').val());

		hubVar.server.connect($('.userName').val());

		$("#Send").click(function () {

			var recipientName = $('.friendUserName').val();
			var sendMessageText = $('.messageText').val();

			if (sendMessageText.length == 0)
				return;

			$.ajax({
				type: 'POST',
				url: '/Chat/SendMessage',
				data: {
					messageText: sendMessageText,
					friendUserName: recipientName
				},
				success: function (result) {
					$('#Send').prop('disabled', true);

					$('#messageText').val(result);
					$('#messageText').prop('readonly', true);

					setTimeout(function () {
						$('#messageText').val("");
						$('#messageText').prop('readonly', false);

						$('#Send').prop("disabled", false);
					}, 1500);
				}
			});

		});
	});

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