<%

'response.write request("userno")
'response.end
'response.redirect "http://e-rma.cipherlab.com.tw:8091/oeb_reason_add.aspx?userno=" & request("userno")
response.redirect "http://e-rma.cipherlab.com.tw:8091/import_oeb.asp?userno=" & request("userno")
%>