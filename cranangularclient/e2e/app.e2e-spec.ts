import { CranAngularClientPage } from './app.po';

describe('cran-angular-client App', () => {
  let page: CranAngularClientPage;

  beforeEach(() => {
    page = new CranAngularClientPage();
  });

  it('should display welcome message', () => {
    page.navigateTo();
    const menu = page.getMenu();
    expect(menu.then(x => x.length)).toBeGreaterThan(2);
  });
});
