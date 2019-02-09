import { AppPage } from './app.po';

let page: AppPage;

fixture('App')
  // .httpAuth(getCredentials())
  .beforeEach(async (t) => {
    page = new AppPage();
  });

test('should display hello message', async (t) => {
  await page.navigateTo();
  const paragraphText = await page.getParagraphText();

  await t.expect(paragraphText).contains('Hello world !');
});
