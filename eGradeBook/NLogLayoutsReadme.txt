Too much information
layout="${date}|${level}|${threadid}|${ActivityId}|mdc:RequestGuid=${mdc:item=RequestGuid}|message=${message}|exception=${exception:format=toString,Data:maxInnerExceptionLevel=10}"

Without mdc, only activityId
layout="${date}|${level}|${threadid}|${ActivityId}|message=${message}|exception=${exception:format=toString,Data:maxInnerExceptionLevel=10}"

Exception layout options
${exception} Only the message of the first exception
${exception:format=message} same
${exception:format=toString} Also print the inner exceptions
${exception:format=toString,Data} print exception data
${exception:format=toString,Data:maxInnerExceptionLevel=10} Limit inner exception levels

${exception:format=@} let's try... I need the JSon layout in order this to work....

${exception:format=Message,Type,ShortType,ToString,Method,StackTrace,Data:separator=\r\n\r\n}

layout="${date}|${level}|${threadid}|${ActivityId}|message=${message}|exception=${exception:format=@}"