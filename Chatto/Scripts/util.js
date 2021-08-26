$(function () {

	var hubVar = $.connection.signalHub;

	$.connection.hub.start().done(function () {
		console.log('connected!' + hubVar.connection.id)
	})

	hubVar.client.showNotification = function (userName, message) {
		document.getElementById('notificationBlock').removeAttribute('hidden');
		document.getElementById('notificationBlock').append(htmlEncode(userName) + ' ' + htmlEncode(message));
		$.fn.playNotificationSound();

		setTimeout(function () {
			window.location.reload();
		}, 2000);
	};

	$.fn.playNotificationSound = function () {
		const audio = new Audio('/Sounds/ping.wav');
		audio.play();
	};

	$.fn.playMessageSound = function () {
		const audio = new Audio('/Sounds/message.wav');
		audio.play();
	};

	hubVar.client.onConnected = function (id, userName, users) {
		for (i = 0; i < users.length; i++) {
			AddUser(users[i].ConnectionId, users[i].UserName);
		}
	};

	hubVar.client.onNewUserConnected = function (id, userName) {
		AddUser(id, userName);
	};

	hubVar.client.onUserDisconnected = function (id, userName) {
		$('#' + id).remove();
	};

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