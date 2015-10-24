INSERT INTO [MailTemplate](Template, TemplateId, Subject, HtmlBody, TextBody,IsActive)
SELECT 'NewRegistration',1,'Welcome to Email utility','<b>Hello UserName~,<br /> <p>Your account with Portalname~ has been activated.</p>
<br /> Signature~</b>',
'Hello UserName~, Your account with Portalname~ has been activated.
Signature~',1
UNION ALL
SELECT 'UserActivation',2,'Account activated of UserName~','<b>Hello UserName~,<br /> <p>Your account with Portalname~ has been activated.</p>
<br /> Signature~</b>',
'Hello UserName~,Your account with Portalname~ has been activated.
Signature~',1
UNION ALL
SELECT 'ForgetUserId',3,'Recover UserId','<b>Hello UserName~,<br /> <p>Your userId registered with Portalname~ is <b>UserId~</b>.</p>
<br /> Signature~</b>',
'Hello UserName~,Your userId registered with Portalname~ is UserId~.
Signature~',1
UNION ALL
SELECT 'ForgetPassword',4,'Recover Password','<b>Hello UserName~,<br /> <p>Your new password to access Portalname~ is <b>Password~</b>.</p>
<br /> Signature~</b>',
'Hello UserName~,Your new password to access Portalname~ is Password~.
Signature~',1
