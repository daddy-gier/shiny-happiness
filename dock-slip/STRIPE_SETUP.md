# Connecting Stripe to SlipKeeper Pro

Follow these steps to go live with payments. Takes about 10 minutes.

---

## Step 1 — Create your Stripe account (if you don't have one)

1. Go to https://stripe.com and sign up (free).
2. Complete identity verification so you can receive payouts.

---

## Step 2 — Create the SlipKeeper Pro product

1. In the Stripe Dashboard, click **Products** in the left sidebar.
2. Click **+ Add product**.
3. Fill in:
   - **Name:** `SlipKeeper Pro`
   - **Description:** `One-time-purchase marina slip management software. Unlimited slips, lifetime license.`
   - **Pricing:** One time · **$499.00 USD**
4. Click **Save product**.

---

## Step 3 — Create a Buy Button

1. In the Stripe Dashboard, click **Payment Links** in the left sidebar.
2. Click **+ New** → select your SlipKeeper Pro product → click **Continue**.
3. (Optional) Under **After payment**, set the redirect URL to:
   `https://your-site-url/dock-slip/success.html`
4. At the top of the page, click the **Buy button** tab.
5. Click **Create buy button**.
6. You'll see a code snippet like:
   ```html
   <stripe-buy-button
     buy-button-id="buy_btn_xxxxxxxxxxxxxxxx"
     publishable-key="pk_live_xxxxxxxxxxxxxxxx">
   </stripe-buy-button>
   ```
7. Copy the `buy-button-id` value and the `publishable-key` value.

---

## Step 4 — Paste your keys into index.html

Open `dock-slip/index.html` and find this section (around line 160):

```html
<stripe-buy-button
  buy-button-id="PASTE_BUY_BUTTON_ID_HERE"
  publishable-key="PASTE_STRIPE_PUBLISHABLE_KEY_HERE">
</stripe-buy-button>
```

Replace the placeholder values:

```html
<stripe-buy-button
  buy-button-id="buy_btn_xxxxxxxxxxxxxxxx"
  publishable-key="pk_live_xxxxxxxxxxxxxxxx">
</stripe-buy-button>
```

Then **delete the setup-banner div** below it (the yellow warning block).

---

## Step 5 — Deploy to GitHub Pages

Push the `dock-slip/` folder to your `maxgier2026-cmyk/slipkeeper-pro` repo, or
enable GitHub Pages on `daddy-gier/shiny-happiness` (Settings → Pages → `main`
branch → `/` root). Your buy page will be live at:

```
https://daddy-gier.github.io/shiny-happiness/dock-slip/
```

Or copy the files directly into the `maxgier2026-cmyk/slipkeeper-pro` repo.

---

## Step 6 — Add a "Buy Now" link from your main sales page

In your existing `index.html` (the main sales page at slipkeeper-pro), add a button
linking to this checkout page:

```html
<a href="https://daddy-gier.github.io/shiny-happiness/dock-slip/" class="buy-btn">
  Buy SlipKeeper Pro — $499
</a>
```

---

## Step 7 — Test with a test card

Before going live, use Stripe's test card `4242 4242 4242 4242` (any future
expiry, any CVC) to make sure the whole flow works. Switch from **Test mode** to
**Live mode** in the Stripe Dashboard when you're ready.

---

## Notes

- **Publishable key** (starts with `pk_live_`) is safe to put in HTML — it's public by design.
- **Secret key** (starts with `sk_live_`) should **never** go in HTML. We don't need it here.
- The Stripe Buy Button handles everything: card collection, PCI compliance, fraud detection.
- You'll receive payouts to your bank account on the schedule you configure in Stripe settings.

---

Questions? Contact Lee: leegier6@gmail.com · 417-380-4041
