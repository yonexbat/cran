import { CranAngularClientPage } from './app.po';

describe('cran-angular-client App', () => {
  let page: CranAngularClientPage;

  beforeEach(() => {
    page = new CranAngularClientPage();
  });

  it('should display welcome message', () => {
    page.navigateTo();
    expect(page.getParagraphText()).toEqual('Welcome to app!');
  });
});
