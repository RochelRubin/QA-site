$(() => {

    $("#like-btn").on('click', () => {
        const questionId = $("#likes-count").data('question-id');
        $.post('/question/addquestionlikes', { questionId }, () => {
            updateLikes();
            $("#like-btn").prop('disabled', true);
        })
    })

    const updateLikes = () => {
        const id = $("#likes-count").data('question-id');
        $.get('/question/getlikes', { id }, function (result) {
            $("#likes-count").text(result)
            
        })

    }


    setInterval(updateLikes, 1000);

});